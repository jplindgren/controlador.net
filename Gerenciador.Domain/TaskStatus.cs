using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain {
    public enum TaskStatus {
        Proposed = 0,
        Open,
        Rejected,
        InProgress,
        Completed,
        Cancelled
    } //class
}
