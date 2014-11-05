using Gerenciador.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Services.Data {
    public class ProjectSummary {
        public Project Project { get; set; }

        public IEnumerable<Gerenciador.Domain.Task> OpenTasks { get; set; }
        public int PercentageOpenTasks { get; set; }
        public IEnumerable<Gerenciador.Domain.Task> ClosedTasks { get; set; }
        public int PercentageClosedTasks { get; set; }
        public IEnumerable<Gerenciador.Domain.Task> CancelledTasks { get; set; }
        public int PercentageCancelledTasks { get; set; }
    }// class
}
