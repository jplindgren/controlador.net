using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain {
    public class Project {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Owner { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public virtual ICollection<Task> Tasks { get;set; }

        private IEnumerable<Task> ValidTasks() {
            return Tasks.Where(x => x.Status != TaskStatus.Cancelled).AsEnumerable();
        }

        public IEnumerable<Task> OpenTasks() {
            return ValidTasks().Where(x => x.Progress != 100).AsEnumerable();
        }

        public IEnumerable<Task> ClosedTasks() {
            return ValidTasks().Where(x => x.Progress == 100).AsEnumerable();
        }

        public IEnumerable<Task> CancelledTasks() {
            return Tasks.Where(x => x.Status == TaskStatus.Cancelled).AsEnumerable();
        }

        public int CalculatePercentageForTasks(decimal numberToEvaluate) {
            if (numberToEvaluate == 0) {
                return 0;
            }
            if (Tasks == null || Tasks.Count() == 0)
                throw new Exception("Não existem tasks para esse projeto, logo o cálculo é impossível");

            var percentage = (numberToEvaluate / Tasks.Count()) * 100;
            return (int)percentage;
        }

        public Task AddTask(string taskName, string taskDescription, RangeDate rangeDate, string username) {
            Task task = Task.CreateTaskAsAdmin(taskName, taskDescription, this.Id, this, rangeDate.Start, rangeDate.End, username);
            this.Tasks.Add(task);
            return task;
        }

        public int AmountEffotrNeeded() {
            return ValidTasks().Count() * 100;
        }
    } //class
}
