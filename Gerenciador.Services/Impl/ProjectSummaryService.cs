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
        private ITaskProgressHistoryRepository taskProgressRepository;

        public ProjectSummaryService(IProjectRepository projectRepository, ITaskProgressHistoryRepository taskProgressRepository) {
            _projectRepository = projectRepository;
            this.taskProgressRepository = taskProgressRepository;
        }

        public ProjectSummary GetProjectSummary(Guid id) {
            var projectSummary = new ProjectSummary();
            var project = _projectRepository.Get(id, "Tasks");
            
            if (project == null) {
                return null; // remember to change to null pattern. Maybe an NonExistentProjectSummary
            }
            projectSummary.Project = project;
            projectSummary.OpenTasks = project.OpenTasks();
            projectSummary.PercentageOpenTasks = project.CalculatePercentageForTasks(projectSummary.OpenTasks.Count()); 
            projectSummary.ClosedTasks = project.ClosedTasks();
            projectSummary.PercentageClosedTasks = project.CalculatePercentageForTasks(projectSummary.ClosedTasks.Count()); 
            projectSummary.CancelledTasks = project.CancelledTasks();
            projectSummary.PercentageCancelledTasks = project.CalculatePercentageForTasks(projectSummary.CancelledTasks.Count()); 
            return projectSummary;
        }

        public void CreateProject(Project project) {
            _projectRepository.Add(project);
        }

        public ProjectUpdateData GetProgressData(Guid projectId) {
            var project = _projectRepository.Get(projectId, "Tasks");

            var progressHistory = taskProgressRepository.GetProgressHistory(projectId);

            var dataPoints = progressHistory.GroupBy(x => new DateTime(x.UpdatedAt.Year, x.UpdatedAt.Month, x.UpdatedAt.Day))
                .Select(x =>
                        new DataPoint(x.Key,
                                      progressHistory.Where(z => new DateTime(z.UpdatedAt.Year, z.UpdatedAt.Month, z.UpdatedAt.Day) <= x.Key)
                                                        .Sum(y => y.Progress),
                                      project.Tasks.Where(x1 => x1.StartDate.Date <= x.Key).Count())
                    ).ToList();
            return new ProjectUpdateData(project.AmountEffotrNeeded(), dataPoints);
        }
    } //class
}
