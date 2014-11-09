using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Services.Data {
    public class ProjectUpdateHistory {
        public DateTime Date { get; set; }
        public int EstimatedProgress { get; set; }
        public int RealProgress { get; set; }
    } //class
}
