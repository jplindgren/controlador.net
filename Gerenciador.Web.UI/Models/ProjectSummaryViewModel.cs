using Gerenciador.Services.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Models {
    public class ProjectSummaryViewModel {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }

        [DataType(DataType.MultilineText)]
        public string ProjectDescription { get; set; }
        public DateTime ProjectCreatedAt { get; set; }
        public DateTime ProjectLastUpdatedAt { get; set; }

        public int NumberOfTasks {
            get { return OpenTasks.Count() + ClosedTasks.Count() + CancelledTasks.Count(); }
        }

        public int PercentageOpenTasks { get; set; }
        public int PercentageClosedTasks { get; set; }
        public int PercentageCancelledTasks { get; set; }

        public IEnumerable<TaskViewModel> OpenTasks { get; set; }
        public IEnumerable<TaskViewModel> LastOpenTasks {
            get { return OpenTasks.OrderByDescending(x => x.CreatedAt).Take(5); }
        }

        public IEnumerable<TaskViewModel> ClosedTasks { get; set; }
        public IEnumerable<TaskViewModel> CancelledTasks { get; set; }
        

        public static ProjectSummaryViewModel FromProjectSummary(ProjectSummary projectSummary){
            return new ProjectSummaryViewModel() {
                ProjectId = projectSummary.Project.Id,
                ProjectName = projectSummary.Project.Name,
                ProjectDescription = projectSummary.Project.Description,
                ProjectCreatedAt = projectSummary.Project.CreatedAt,
                ProjectLastUpdatedAt = projectSummary.Project.LastUpdatedAt,
                PercentageOpenTasks = projectSummary.PercentageOpenTasks,
                PercentageClosedTasks = projectSummary.PercentageClosedTasks,
                PercentageCancelledTasks = projectSummary.PercentageCancelledTasks,
                OpenTasks = TaskViewModel.FromTask(projectSummary.OpenTasks),
                ClosedTasks = TaskViewModel.FromTask(projectSummary.ClosedTasks),
                CancelledTasks = TaskViewModel.FromTask(projectSummary.CancelledTasks)
            };
        }
    } //class
}