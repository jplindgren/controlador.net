using Gerenciador.Domain.UserContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain.Todo {
    public class TodoItem {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Order { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile UserProfile { get; set; }
        public int UserId { get; set; }

        public bool Done { get; set; }
    } //class
}
