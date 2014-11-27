using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Gerenciador.Domain {
    public class TransitionalItem {
        [DisplayFormat(DataFormatString = "{0:d}")]
        public virtual DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString="{0:d}")]
        public virtual DateTime Deadline { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public virtual DateTime? EndDate { get; set; }

        public bool IsDelayed() {
            return DateTime.Now > Deadline && EndDate > Deadline;
        }
    }// class
}
