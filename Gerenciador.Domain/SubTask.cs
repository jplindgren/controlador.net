using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain {
    public class SubTask {
        public SubTask() {}
        public SubTask(string name, DateTime startDate, DateTime expectedEndDate) {
            Name = name;
            CreatedAt = DateTime.Now;
            StartDate = startDate;
            ExpectedEndDate = expectedEndDate;
            Status = TaskStatus.Open;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public TaskStatus Status { get; set; }        

        [ForeignKey("TaskId")]
        public Task Task{ get; set; }
        public Guid TaskId { get; set; }
    } //class
}
