using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork.Interface;
using Gerenciador.Services.Data;
using Gerenciador.Services.Interface;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Services.Impl {
    public class ProjectSummaryService : IProjectSummaryService {
        private IProjectRepository _projectRepository;
        public ProjectSummaryService(IProjectRepository projectRepository) {
            _projectRepository = projectRepository;
        }

        public ProjectSummary GetProjectSummary(Guid id) {
            var projectSummary = new ProjectSummary();
            var project = _projectRepository.Get(id);
            if (project == null) {
                return null; // remember to change to null pattern. Maybe an NonExistentProjectSummary
            }
            projectSummary.Project = project;
            projectSummary.OpenTasks = project.OpenTasks();
            projectSummary.PercentageOpenTasks = project.CalculatePercentageForTasks(projectSummary.OpenTasks); 
            projectSummary.ClosedTasks = project.ClosedTasks();
            projectSummary.PercentageClosedTasks = project.CalculatePercentageForTasks(projectSummary.ClosedTasks); 
            projectSummary.CancelledTasks = project.CancelledTasks();
            projectSummary.PercentageCancelledTasks = project.CalculatePercentageForTasks(projectSummary.CancelledTasks); 
            return projectSummary;
        }

        public void CreateProject(Project project) {
            _projectRepository.Add(project);
        }
    } //class
}
