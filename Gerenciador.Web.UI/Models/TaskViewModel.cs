using Gerenciador.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Models {
    public class TaskViewModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public int Progress { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? EndDate { get; set; }        
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Deadline { get; set; }

        public virtual IList<SubTask> SubTasks { get; set; }

        //public Pager Pager { get; set; }
    }
}