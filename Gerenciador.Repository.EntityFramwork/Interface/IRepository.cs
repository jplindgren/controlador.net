using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork.Interface {
    public interface IRepository<T> {
        IQueryable<T> GetAll();
        T Get(Guid id);
        void Add(T entity);
        void Remove(T entity);
        void ExecuteCommand(string sql, params object[] parameters);
        void SaveChanges();
    }
}
