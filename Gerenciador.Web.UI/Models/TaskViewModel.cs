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

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = " ")]
        [Display(Name = "Data de início")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Data de término")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = " ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Prazo")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        public IList<SubTaskTaskDetailViewModel> SubTasks { get; set; }

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
                SubTasks = SubTaskTaskDetailViewModel.FromSubTask(task.GetOrderedSubtasks())
            };
        }

        public static IList<TaskViewModel> FromTask(IEnumerable<Task> tasks) {
            if (tasks == null)
                throw new ArgumentNullException("tasks");

            return tasks.Select(x => FromTask(x)).ToList();
        }


        public class SubTaskTaskDetailViewModel {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public DateTime CreatedAt { get; set; }

            [DisplayFormat(DataFormatString = "{0:d}")]
            public DateTime StartDate { get; set; }
            [DisplayFormat(DataFormatString = "{0:d}")]
            public DateTime ExpectedEndDate { get; set; }
            public TaskStatus Status { get; set; }
            public Guid TaskId { get; set; }

            public static SubTaskTaskDetailViewModel FromSubTask(SubTask subtask) {
                return new SubTaskTaskDetailViewModel() {
                    Id = subtask.Id,
                    Name = subtask.Name,
                    CreatedAt = subtask.CreatedAt,
                    ExpectedEndDate = subtask.ExpectedEndDate,
                    StartDate = subtask.StartDate,
                    Status = subtask.Status,
                    TaskId = subtask.TaskId
                };
            }

            public static IList<SubTaskTaskDetailViewModel> FromSubTask(IEnumerable<SubTask> subtasks) {
                if (subtasks == null)
                    throw new ArgumentNullException("tasks");

                return subtasks.Select(x => FromSubTask(x)).ToList();
            }

        }
    } //class
}