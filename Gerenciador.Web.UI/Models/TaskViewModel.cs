using Gerenciador.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Models {
    public class TaskViewModel {
        public Guid Id { get; set; }

        [Required(ErrorMessage = " ")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public int Progress { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessage = " ")]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = " ")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Deadline { get; set; }

        public virtual IList<SubTask> SubTasks { get; set; }

        //public Pager Pager { get; set; }

        public static TaskViewModel FromTask(Task task){
            return new TaskViewModel() {
                Id = task.Id,
                Name = task.Name,
                CreatedAt = task.CreatedAt,
                Deadline = task.Deadline,
                Description = task.Description,
                EndDate = task.EndDate,
                LastUpdatedAt = task.LastUpdatedAt,
                Progress = task.Progress,
                ProjectId = task.ProjectId,
                StartDate = task.StartDate,
                Status = task.Status,
                SubTasks = task.GetOrderedSubtasks()
            };
        }
    } //class
}