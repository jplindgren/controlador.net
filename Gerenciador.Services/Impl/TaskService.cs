using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Services.Impl {
    public class TaskService {
        private ITaskRepository taskRepository;
        public TaskService(ITaskRepository taskRepository) {
            this.taskRepository = taskRepository;
        }

        public IList<SubTask> GetSubTasks(Guid taskId) {
            var task = this.taskRepository.Get(taskId);
            return task.SubTasks.ToList();
        }

        public SubTask SetSubTaskDone(Guid taskId, Guid subTaskId) {
            var task = this.taskRepository.Get(taskId);
            var subtask = task.SubTasks.Where(x => x.Id == subTaskId).First();
            subtask.Status = Domain.TaskStatus.Completed;
            return subtask;
        }
    } // class
}
