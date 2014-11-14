using Gerenciador.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerenciador.Repository.EntityFramwork.Interface {
    public interface ITaskProgressHistoryRepository : IRepository<TaskProgressHistory> {
        IList<TaskProgressHistory> GetProgressHistory(Guid projectId);
    } //interface
}
