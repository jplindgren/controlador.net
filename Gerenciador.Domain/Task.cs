using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain {
    public class Task : TransitionalItem{
        public Task() {}
        public Task(string name, string description, Guid projectId, Project project, DateTime start, DateTime deadline, string createdBy) {
            Name = name;
            Description = description;
            ProjectId = projectId;
            Project = project;

            Progress = 0;
            CreatedAt = DateTime.Now;
            LastUpdatedAt = DateTime.Now;
            CreatedBy = createdBy;
            LastUpdatedBy = createdBy;

            StartDate = start;
            Deadline = deadline;

            Status = TaskStatus.Open; //TODO: In future change to create methods an implement propose method by customers
        }

        public static Task CreateTask(string name, string description, Guid projectId, Project project, DateTime start, DateTime deadline, string createdBy) {
            var task = new Task(name, description, projectId, project, start, deadline, createdBy);
            task.Status = TaskStatus.Proposed;
            return task;
        }

        public static Task CreateTaskAsAdmin(string name, string description, Guid projectId, Project project, DateTime start, DateTime deadline, string createdBy) {
            var task = new Task(name, description, projectId, project, start, deadline, createdBy);
            //task.Status = TaskStatus.Open;
            task.CreateProgressHistoryFromThatTask(0, start);
            return task;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        private string name;
        [Required]
        public string Name { 
            get { return name; }
                set {
                var oldValue = name;
                name = value;
                // Call OnPropertyUpdated whenever the property is updated
                if (oldValue != name)
                    OnPropertyUpdated("Nome", oldValue, name);
            }
        }

        private string description;
        [DataType(DataType.MultilineText)]
        public string Description {
            get { return description; }
            set {
                var oldValue = description;
                description = value;
                // Call OnPropertyUpdated whenever the property is updated
                if (oldValue != description)
                    OnPropertyUpdated("Descricao", oldValue, description);
            } 
        }

        private TaskStatus status;
        [Required]
        public TaskStatus Status { 
            get { return status; }
            set {
                var oldValue = status;
                status = value;
                // Call OnPropertyUpdated whenever the property is updated
                if (oldValue != status)
                    OnPropertyUpdated("Status", oldValue, status);
            }
        }

        public override DateTime Deadline {
            get {
                return base.Deadline.Date;
            }
            set {
                var oldValue = base.Deadline.Date;
                base.Deadline = value.Date;
                // Call OnPropertyUpdated whenever the property is updated
                if (oldValue != base.Deadline)
                    OnPropertyUpdated("Prazo", oldValue, base.Deadline);                
            }
        }

        public override DateTime StartDate {
            get {
                return base.StartDate.Date;
            }
            set {
                var oldValue = base.StartDate.Date;
                base.StartDate = value.Date;
                // Call OnPropertyUpdated whenever the property is updated
                if (oldValue != base.StartDate)
                    OnPropertyUpdated("Data de Início", oldValue, base.StartDate);
            }
        }

        public override DateTime? EndDate {
            get {
                return base.EndDate;
            }
            set {
                var oldValue = base.EndDate;
                base.EndDate = value;
                // Call OnPropertyUpdated whenever the property is updated
                if (oldValue != base.EndDate)
                    OnPropertyUpdated("Data Término", oldValue, base.EndDate);
            }
        }

        public int Progress { get; set; }
        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        [Required]
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual ICollection<SubTask> SubTasks { get; set; }
        public virtual ICollection<TaskProgressHistory> ProgressHistory { get; set; }

        public IList<SubTask> GetOrderedSubtasks() {
            return SubTasks.OrderByDescending(x => x.CreatedAt).ToList();
        }

        public void CreateProgressHistoryFromThatTask(int progressValue, DateTime date) {
            if (ProgressHistory == null)
                ProgressHistory = new List<TaskProgressHistory>();
            ProgressHistory.Add(new TaskProgressHistory() { Progress = progressValue, Task = this, UpdatedAt = date, ProjectId = this.ProjectId });
        }

        public void UpdateProgress(int progress, string username) {
            var oldProgress = Progress;
            Progress = progress;
            var today = DateTime.Now;
            if (progress == 100) {
                Status = TaskStatus.Completed;
                EndDate = today;
            } else {
                Status = TaskStatus.Open;
                EndDate = today;
            }
            LastUpdatedAt = today;
            LastUpdatedBy = username;
            var valueUpdated = Progress - oldProgress;
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

        public void Update(string name, string description, DateTime startDate, DateTime deadline, string username) {
            if (string.IsNullOrEmpty(username))
                throw new InvalidOperationException("Para atualizar uma task é preciso fornecer o usuário que está efetuando a operação.");

            LastUpdatedAt = DateTime.Now;
            LastUpdatedBy = username;

            Name = name;
            Description = description;
            StartDate = startDate;
            Deadline = deadline;            
        }

        public delegate void PropertyUpdatedEventHandler(object sender, PropertyUpdatedEventArgs args);
        public virtual event PropertyUpdatedEventHandler PropertyUpdated;
        protected void OnPropertyUpdated(string name, object oldValue, object newValue) {
            PropertyUpdatedEventHandler handler = PropertyUpdated;
            if (handler != null) {
                handler(this, new PropertyUpdatedEventArgs(name, oldValue, newValue));
            }
        }
    } //class

    public class PropertyUpdatedEventArgs : PropertyChangedEventArgs {
        public PropertyUpdatedEventArgs(string propertName, object oldValue, object newValue)
            : base(propertName) {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public object NewValue { get; private set; }
        public object OldValue { get; private set; }
    }
}
