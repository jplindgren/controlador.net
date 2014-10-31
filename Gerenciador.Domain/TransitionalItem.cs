using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerenciador.Domain {
    public class TransitionalItem {
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsDelayed() {
            return DateTime.Now > Deadline && EndDate > Deadline;
        }
    }// class
}
