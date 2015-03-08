using Gerenciador.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork.Interface {
    public interface IProjectRepository : IRepository<Project>{
        Domain.Task GetTask(Guid projectId, Guid taskId, params string[] includes);
        Task<IEnumerable<Project>> GetActiveProjectsAsync();
    } //class
}
