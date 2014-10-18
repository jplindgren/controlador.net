using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain {
    public class Comment {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:ss}")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("TaskId")]
        public Task Task { get; set; }
        public Guid? TaskId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public Guid? ProjectId { get; set; }
    } //class
}
