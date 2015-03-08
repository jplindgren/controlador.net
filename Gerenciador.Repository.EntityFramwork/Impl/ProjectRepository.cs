using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Gerenciador.Repository.EntityFramwork.Impl {
    public class ProjectRepository : Repository<Project>, IProjectRepository {
        public ProjectRepository(IDataContext dataContext)
            : base(dataContext) {
            _dataContext = dataContext;
        }

        //public IList<Project> GetProjectsByOwner(string owner) {
        //    return this.GetAll().Where(x => x.Owner == owner).ToList();
        //}

        public Domain.Task GetTask(Guid projectId, Guid taskId, params string[] includes) {
            var result = _dataContext.Set<Domain.Task>()
                .Where(t => t.ProjectId == projectId && t.Id == taskId)
                .Include(t => t.SubTasks).ToList();
            if (result.Count() > 1)
                throw new Exception("More tha one task to the specified id");
            
            return result.First();
        }

        #region Async Methods
        public async Task<IEnumerable<Project>> GetActiveProjectsAsync() {
            return await this.GetAll().Where(x => x.Status == ProjectStatus.InProgress || x.Status == ProjectStatus.Open)
                                .OrderBy(x => x.CreatedAt)
                                .ToListAsync();
        }
        #endregion
    } //class
}
