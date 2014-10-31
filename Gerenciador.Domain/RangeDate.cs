using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain {
    public class RangeDate {
        public RangeDate(DateTime start, DateTime end) {
            this.Start = start;
            this.End = end;
        }
        public DateTime Start;
        public DateTime End;
    }
}
