using Gerenciador.Domain;
using Gerenciador.Domain.UserContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Models {
    public class AdminDashboardViewModel {
        public IList<Project> LastActivesProjects { get; set; }
        public int NumberActiveProjects { get; set; }

        public IList<TaskDashboardViewModel> NextTasks { get; set; }
        public IList<UserProfile> Users { get; set; }

        //When create an RegularUserDashBoard, remember to create an BaseUserDashboardViewModel and put this property to there
        public UserProfile CurrentUser { get; set; }

        public int NumberOfNewMessages { get; set; }
        public int NumberOfTickets { get; set; }
        public int NumberOfProposals { get; set; }

        public class TaskDashboardViewModel {
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
            public Project Project { get; set; }

            [DisplayFormat(DataFormatString = "{0:d}")]
            [DataType(DataType.Date)]
            //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Required(ErrorMessage = " ")]
            [Display(Name = "Data de início")]
            public DateTime StartDate { get; set; }

            [Required(ErrorMessage = " ")]
            [DisplayFormat(DataFormatString = "{0:d}")]
            [Display(Name = "Prazo")]
            public DateTime Deadline { get; set; }

            public static TaskDashboardViewModel FromTask(Task task) {
                return new TaskDashboardViewModel() {
                    Id = task.Id,
                    Name = task.Name,
                    Deadline = task.Deadline,
                    Description = task.Description,
                    Progress = task.Progress,
                    ProjectId = task.ProjectId,
                    Project = task.Project,
                    StartDate = task.StartDate,
                    Status = task.Status
                };
            }

            public static IList<TaskDashboardViewModel> FromTask(IEnumerable<Task> tasks) {
                if (tasks == null)
                    throw new ArgumentNullException("tasks");

                return tasks.Select(x => FromTask(x)).ToList();
            }
        }
    } //class
}