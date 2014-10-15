using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain {
    public class Project {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public virtual ICollection<Task> Tasks { get;set; }

        public IEnumerable<Task> OpenTasks() {
            return Tasks.Where(x => x.Progress != 100).AsEnumerable();
        }

        public IEnumerable<Task> ClosedTasks() {
            return Tasks.Where(x => x.Progress == 100).AsEnumerable();
        }

        public IEnumerable<Task> CancelledTasks() {
            return new List<Task>();
        }

        public int CalculatePercentageForTasks(IEnumerable<Task> tasks) {
            if (tasks == null || tasks.Count() == 0) {
                return 0;
            }
            if (Tasks == null || Tasks.Count() == 0)
                throw new Exception("Não existem tasks para esse projeto, logo o cálculo é impossível");

            var percentage = (tasks.Count() / Tasks.Count()) * 100;
            return percentage;
        }
    } //class
}
