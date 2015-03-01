using Gerenciador.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Models.TaskViewModels {
    public class TaskWidgetViewModel {
        public Guid Id { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        public TaskStatus Status { get; set; }

        [Display(Name = "Progresso")]
        public int Progress { get; set; }
        public Guid ProjectId { get; set; }

        [Display(Name = "Data de término")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Criado em")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = " ")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Prazo")]
        public DateTime Deadline { get; set; }

        public static TaskWidgetViewModel FromTask(Task task) {
            return new TaskWidgetViewModel() {
                Id = task.Id,
                Name = task.Name,
                Deadline = task.Deadline,
                Description = task.Description,
                CreatedAt = task.CreatedAt,
                EndDate = task.EndDate,
                Progress = task.Progress,
                ProjectId = task.ProjectId,
                Status = task.Status
            };
        }

        public static IList<TaskWidgetViewModel> FromTask(IEnumerable<Task> tasks) {
            if (tasks == null)
                throw new ArgumentNullException("tasks");

            return tasks.Select(x => FromTask(x)).ToList();
        }
    } //class
}