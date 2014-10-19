using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain.Snapshot {
    public class EventSnapshot {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime EventDate { get; set; }
        public string Subject { get; set; }
        public string Author { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? ResourceId { get; set; }
        public string Action { get; set; }
        public string Resource { get; set; }
    } //class
}
