using Gerenciador.Domain;
using Gerenciador.Domain.Snapshot;
using Gerenciador.Repository.EntityFramwork.Interface;
using Gerenciador.Services.Hangfire;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Gerenciador.Services.Impl {
    public class ProjectService {
        private IProjectRepository _projectRepository;
        private HistoryService _historyService;

        public ProjectService(IProjectRepository projectRepository, HistoryService historyService) {
            _projectRepository = projectRepository;
            _historyService = historyService;
        }

        public Project GetProject(Guid id) {
            //Expression<Func<Project, ICollection<Task>>> includeExpression1 = x => { return x.Tasks; };
            //var includeExpressions = new List<Expression<Func<Project, ICollection<Task>>>>{ includeExpression1 };

            return _projectRepository.Get(id, "Tasks", "Tasks.SubTasks");
        }

        public Task GetTask(Guid projectId, Guid taskId) {
            return _projectRepository.GetTask(projectId, taskId , "Tasks.SubTasks");
        }

        public async System.Threading.Tasks.Task CreateTask(Project project, string username, string taskName, string taskDescription, RangeDate rangeDate, System.Threading.CancellationToken token) {
            var task = project.AddTask(taskName, taskDescription, rangeDate, username);
            await _projectRepository.SaveChangesAsync(token);

            var snapshot = new EventSnapshotBuilder().ForAction("Create").Consume(task).Create();
            _historyService.CreateEntry(snapshot);
        }

        public void UpdateTask(Task task, int progress, string username) {
            var valueUpdated = progress - task.Progress;

            task.UpdateProgress(progress, username);

            var snapshot = new EventSnapshotBuilder().ForAction("UpdateProgress").Consume(task).Create();
            _historyService.CreateEntry(snapshot);

            //DelayedJobs.Execute(() => CreateProgressHistoryFromThatTask(task.ProjectId, task.Id, valueUpdated, DateTime.Now));
            //BackgroundJob.Enqueue<ProjectService>(x => x.CreateProgressHistoryFromThatTask(task.ProjectId, task.Id, valueUpdated, DateTime.Now));
        }

        public void CreateProgressHistoryFromThatTask(Guid projetId, Guid taskId, int valueUpdated, DateTime today){
            var project = _projectRepository.Get(projetId);
            var task = project.Tasks.Where(x => x.Id == taskId).FirstOrDefault();
            task.CreateProgressHistoryFromThatTask(valueUpdated, today);
            _projectRepository.SaveChanges();
        }

        public async System.Threading.Tasks.Task CreateSubTask(Task task, SubTask subtask, string username) {
            task.SubTasks.Add(subtask);
            await _projectRepository.SaveChangesAsync();

            var snapshot = new EventSnapshotBuilder().ForAction("Create").ForUser(username).Consume(subtask).Create();
            _historyService.CreateEntry(snapshot);
        }

        public async System.Threading.Tasks.Task<object> GetLastActiveProjectsAsync() {
            var activeProjects = await _projectRepository.GetActiveProjectsAsync();
            var numberActiveProjects = activeProjects.Count();
            var lastActiveProjects = activeProjects.Take(5).ToList();

            dynamic result = new ExpandoObject();
            result.LastActiveProjects = lastActiveProjects;
            result.NumberOfActiveProjects = numberActiveProjects;

            return result;
        }
    } //class
}
