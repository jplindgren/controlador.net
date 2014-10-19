using Gerenciador.Domain;
using Gerenciador.Domain.Snapshot;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return _projectRepository.Get(id);
        }

        public void CreateTask(Project project, string username, string taskName, string taskDescription) {
            var task = project.AddTask(taskName, taskDescription);
            _projectRepository.SaveChanges();

            var snapshot = new EventSnapshotBuilder().ForAction("Create").ForUser(username).Consume(task).Create();
            _historyService.CreateEntry(snapshot);
        }

        public void UpdateTask(Task task, int progress, string username) {
            task.UpdateProgress(progress);
            var snapshot = new EventSnapshotBuilder().ForAction("Update").ForUser(username).Consume(task).Create();
            _historyService.CreateEntry(snapshot);
        }
    } //class
}
