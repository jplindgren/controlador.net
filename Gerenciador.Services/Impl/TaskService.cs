using Gerenciador.Domain;
using Gerenciador.Domain.Snapshot;
using Gerenciador.Repository.EntityFramwork.Interface;
using Gerenciador.Services.Hangfire;
using Hangfire;
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


        public Task UpdateTask(Guid id, string name, string description, DateTime startDate, DateTime deadline, string username) {
            var task = GetTask(id);
            task.PropertyUpdated += Task_PropertyChanged;
            task.Update(name, description, startDate, deadline, username);
            return task;
        }

        void Task_PropertyChanged(object sender, PropertyUpdatedEventArgs e) {
            var snapshotBuilder = new EventSnapshotBuilder()
                .ForAction("Update")
                .UsingContent(string.Format("A propriedade {0} foi atualizada de '{1}' para '{2}'", e.PropertyName, e.OldValue, e.NewValue))
                .Consume((Task)sender);
            //historyService.CreateEntry(snapshotBuilder.Create());

            //DelayedJobs.Execute(() => CreateProgressHistoryFromThatTask(task.ProjectId, task.Id, valueUpdated, DateTime.Now));
            BackgroundJob.Enqueue<HistoryBackgroundService>(x => x.CreateEntry(snapshotBuilder.Create()));
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
