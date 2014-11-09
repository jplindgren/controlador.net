using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain {
    public class TaskProgressHistory {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Progress { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid TaskId { get; set; }
        [ForeignKey("TaskId")]
        public Task Task { get; set; }

        public Guid ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }//class
}
