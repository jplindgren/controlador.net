using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Data.Entity.Core.Metadata.Edm;

namespace Gerenciador.Repository.EntityFramwork.Impl {
    public class Repository<T> : IRepository<T> where T : class {
        public IDataContext _dataContext { get; set; }

        public Repository(IDataContext dataContext) {
            _dataContext = dataContext;
        }
        
        public IQueryable<T> GetAll() {
            return this.DataSource();
        }

        public T Get(Guid id, params string[] includes) {
            //return _dataContext.Set<T>().Find(id);
            return GetWithIncludes(id, includes);
        }

        public void Add(T entity) {
            _dataContext.Set<T>().Add(entity);
        }

        public void Remove(T entity) {
            _dataContext.Set<T>().Remove(entity);
        }

        public void ExecuteCommand(string sql, params object[] parameters) {
            _dataContext.ExecuteCommand(sql, parameters);
        }

        public void SaveChanges() {
            _dataContext.SaveChanges();
        }

        #region Private Helpers

        /// <summary>
        /// We can't use Find method with include, because it returns even entities that are not in the database yet, but are in state Added. 
        /// So we have to use Where and First, but our generic repository does not know the Id field of the entity to put in a where clause. 
        /// So we have to discover in  runtime.
        /// </summary>
        /// <param name="idValue">value from the id of the entity you want to find</param>
        /// <param name="includes">arrays with collections entities to include</param>
        /// <returns></returns>
        private T GetWithIncludes(Guid idValue, params string[] includes) {
            ObjectContext objectContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
            ObjectSet<T> objectSet = objectContext.CreateObjectSet<T>();

            var keyName = objectSet.EntitySet.ElementType.KeyMembers.Select(k => k.Name).FirstOrDefault();

            //var test = _dataContext.Set<T>().AsQueryable().Include("Tasks").Where(GetExpression(keyName, idValue)).FirstOrDefault();

            var query = IncludeObjectGraph(_dataContext.Set<T>().AsQueryable(), includes);
            var evaluatedResult = query.Where(GetExpression(keyName, idValue)).ToList();
            if (evaluatedResult.Count() > 1)
                throw new InvalidOperationException("More than on register with same key");
            return evaluatedResult.First();
        }

        private IEnumerable<T> QueryObjectGraph(Expression<Func<T, bool>> filter, params string[] includes) {
            var query = GetAll().Where(filter);
            includes.Select(x => query = query.Include(x));
            return query;
        }

        private IQueryable<T> IncludeObjectGraph(IQueryable<T> query, params string[] includes) {
            if (includes == null || includes.Count() == 0)
                return query;
            foreach (var include in includes) {
                query = query.Include(include);
            }            
            //includes.Select(x => query = query.Include(x));
            return query;
        }

        /// <summary>
        /// Returns expression to use in expression trees, like where statements. For example query.Where(GetExpression("IsDeleted", typeof(boolean), false));
        /// </summary>
        /// <param name="propertyName">The name of the property. Either boolean or a nulleable typ</param>
        private Expression<Func<T, bool>> GetExpression(string propertyName, object value) {
            var param = Expression.Parameter(typeof(T));
            var actualValueExpression = Expression.Property(param, propertyName);

            var lambda = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(actualValueExpression,
                    Expression.Constant(value)),
                param);

            return lambda;
        }

        protected IQueryable<T> DataSource() {
            var query = _dataContext.Set<T>().AsQueryable<T>();
            var property = typeof(T).GetProperty("Deleted");

            if (property != null) {
                query = query.Where(GetExpression("Deleted", null));
            }

            return query;
        }
        #endregion

    } //class

    internal static class RuntimeFailureMethods {
        private static readonly Regex _isNotNull = new Regex(
            @"^\s*(@?\w+)\s*\!\=\s*null\s*$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _isNullOrWhiteSpace = new Regex(
            @"^\s*\!\s*string\s*\.\s*IsNullOrWhiteSpace\s*\(\s*(@?[\w]+)\s*\)\s*$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        [DebuggerStepThrough]
        public static void Requires(bool condition, string userMessage, string conditionText) {
            if (!condition) {
                Match match;

                if (((match = _isNotNull.Match(conditionText)) != null)
                    && match.Success) {
                    throw new ArgumentNullException(match.Groups[1].Value);
                }

                if (((match = _isNullOrWhiteSpace.Match(conditionText)) != null)
                    && match.Success) {
                    throw new ArgumentNullException(match.Groups[1].Value);
                }

                throw new Exception(conditionText + " - " + userMessage);
            }
        }
    }
}
