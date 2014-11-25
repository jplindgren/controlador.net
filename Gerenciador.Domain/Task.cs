﻿using System;
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
            task.CreateProgressHistoryFromThatTask(0, start);
            return task;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public TaskStatus Status { get; set; }
        public int Progress { get; set; }
        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

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

        public void UpdateProgress(int progress) {
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
            var valueUpdated = Progress - oldProgress;
            //CreateProgressHistoryFromThatTask(valueUpdated, today);
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
