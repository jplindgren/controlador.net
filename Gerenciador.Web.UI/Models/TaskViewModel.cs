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
        [Display(Name="Nome")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        public TaskStatus Status { get; set; }

        [Display(Name = "Progresso")]
        public int Progress { get; set; }
        public Guid ProjectId { get; set; }

        [Display(Name = "Criado em")]
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = " ")]
        [Display(Name = "Data de início")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Data de término")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = " ")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Prazo")]
        public DateTime Deadline { get; set; }

        private IList<SubTask> subTasks;
        public virtual IList<SubTask> SubTasks { 
            get {
                return subTasks.OrderByDescending(x => x.CreatedAt).ToList();
            } 
            set { subTasks = value; } 
        }

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

        public static IEnumerable<TaskViewModel> FromTask(IEnumerable<Task> tasks) {
            if (tasks == null)
                throw new ArgumentNullException("tasks");

            return  tasks.Select(x => new TaskViewModel() {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                Deadline = x.Deadline,
                Description = x.Description,
                EndDate = x.EndDate,
                LastUpdatedAt = x.LastUpdatedAt,
                Progress = x.Progress,
                ProjectId = x.ProjectId,
                StartDate = x.StartDate,
                Status = x.Status,
                SubTasks = x.GetOrderedSubtasks()
            }).AsEnumerable();
        }
    } //class
}