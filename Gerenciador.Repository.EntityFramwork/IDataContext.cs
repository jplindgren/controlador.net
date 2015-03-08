using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork {
    public interface IDataContext {
        System.Data.Entity.IDbSet<T> Set<T>() where T : class;
        void ExecuteCommand(string command, params object[] parameters);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    } //interface
}
