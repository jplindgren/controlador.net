using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork.Impl {
    public class ProjectRepository : Repository<Project>, IProjectRepository {
        public ProjectRepository(IDataContext dataContext)
            : base(dataContext) {
            _dataContext = dataContext;
        }

        public IList<Project> GetProjectsByOwner(string owner) {
            return this.GetAll().Where(x => x.Owner == owner).ToList();
        }
    } //class
}
