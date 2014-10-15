using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerenciador.Services.Impl {
    public class ProjectService {
        private IProjectRepository _projectRepository;
        public ProjectService(IProjectRepository projectRepository) {
            _projectRepository = projectRepository;
        }

        public Project GetProject(Guid id) {
            return _projectRepository.Get(id);
        }
    } //class
}
