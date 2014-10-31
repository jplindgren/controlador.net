using Gerenciador.Domain;
using Gerenciador.Domain.Snapshot;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerenciador.Services.Impl {
    public class TaskService {
        private ITaskRepository taskRepository;
        private HistoryService historyService;
        public TaskService(ITaskRepository taskRepository, HistoryService historyService) {
            this.taskRepository = taskRepository;
            this.historyService = historyService;
        }

        public Task GetTask(Guid taskId) {
            return this.taskRepository.Get(taskId);
        }

        public SubTask GetSubTask(Guid taskId, Guid subTaskId) {
            var task = this.taskRepository.Get(taskId);
            var subtask = task.SubTasks.Where(x => x.Id == subTaskId).First();
            return subtask;
        }

        public IList<SubTask> GetSubTasks(Guid taskId) {
            var task = this.taskRepository.Get(taskId);
            return task.GetOrderedSubtasks();
        }

        public SubTask ChangeSubTaskStatus(Guid taskId, Guid subTaskId, Domain.TaskStatus subTaskStatus, string username) {
            var task = this.taskRepository.Get(taskId);
            var subtask = task.SubTasks.Where(x => x.Id == subTaskId).First();
            var oldStatus = subtask.Status;
            subtask.Status = subTaskStatus;

            var snapshotBuilder = new EventSnapshotBuilder()
                .ForUser(username)
                .ForAction("Update")
                .UsingContent(string.Format("A propriedade status foi atualizada de {0} para {1}", oldStatus, subTaskStatus))
                .Consume(subtask);
            historyService.CreateEntry(snapshotBuilder.Create());

            return subtask;
        }
    } // class
}
