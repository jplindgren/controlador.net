using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain {
    public class Task {
        public Task() {}
        public Task(string name, string description, Guid projectId, Project project) {
            Name = name;
            Description = description;
            ProjectId = projectId;
            Project = project;

            Done = false;
            Progress = 0;
            CreatedAt = DateTime.Now;
            LastUpdatedAt = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool Done { get; set; }
        public int Progress { get; set; }
        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    } //class
}
