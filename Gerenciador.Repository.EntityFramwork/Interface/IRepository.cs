using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork.Interface {
    public interface IRepository<T> {
        IQueryable<T> GetAll();
        T Get(Guid id, params string[] includes);
        void Add(T entity);
        void Remove(T entity);
        void ExecuteCommand(string sql, params object[] parameters);
        void SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken token);
        Task<int> SaveChangesAsync();
    }
}
