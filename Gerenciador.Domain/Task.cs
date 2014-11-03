using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain {
    public class Task : TransitionalItem{
        public Task() {}
        public Task(string name, string description, Guid projectId, Project project, DateTime start, DateTime deadline) {
            Name = name;
            Description = description;
            ProjectId = projectId;
            Project = project;

            Progress = 0;
            CreatedAt = DateTime.Now;
            LastUpdatedAt = DateTime.Now;

            StartDate = start;
            Deadline = deadline;

            Status = TaskStatus.Open; //TODO: In future change to create methods an implement propose method by customers
        }

        public static Task CreateTask(string name, string description, Guid projectId, Project project, DateTime start, DateTime deadline) {
            var task = new Task(name, description, projectId, project, start, deadline);
            task.Status = TaskStatus.Proposed;
            return task;
        }

        public static Task CreateTaskAsAdmin(string name, string description, Guid projectId, Project project, DateTime start, DateTime deadline) {
            var task = new Task(name, description, projectId, project, start, deadline);
            task.Status = TaskStatus.Open;
            return task;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public TaskStatus Status { get; set; }
        public int Progress { get; set; }
        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public virtual ICollection<SubTask> SubTasks { get; set; }

        public IList<SubTask> GetOrderedSubtasks() {
            return SubTasks.OrderByDescending(x => x.CreatedAt).ToList();
        }

        public void UpdateProgress(int progress) {
            Progress = progress;
            if (progress == 100) {
                Status = TaskStatus.Completed;
                EndDate = DateTime.Now;
            } else {
                Status = TaskStatus.Open;
                EndDate = null;
            }
            LastUpdatedAt = DateTime.Now;
        }

        public void AddSubTask(SubTask subTask) {
            SubTasks.Add(subTask);
        }

        public bool IsDelayed() {
            return base.IsDelayed() && !IsDone();
        }

        public bool IsDone() {
            return (Progress == 100);
        }
    } //class
}
