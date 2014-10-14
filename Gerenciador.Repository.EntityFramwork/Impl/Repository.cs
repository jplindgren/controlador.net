using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork.Impl {
    public class Repository<T> : IRepository<T> where T : class {
        public IDataContext _dataContext { get; set; }

        public Repository(IDataContext dataContext) {
            _dataContext = dataContext;
        }
        
        public IQueryable<T> GetAll() {
            return this.DataSource();
        }

        public T Get(Guid id) {
            return _dataContext.Set<T>().Find(id);
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
}
