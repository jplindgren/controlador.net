using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Services.Impl {
    public class ProjectSummaryService : Gerenciador.Services.Impl.IProjectSummaryService {
        private IProjectRepository _projectRepository;
        public ProjectSummaryService(IProjectRepository projectRepository) {
            _projectRepository = projectRepository;
        }

        public object GetProjectSummary(Guid id) {
            return _projectRepository.Get(id);
        }
    } //class
}
