using Gerenciador.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Services.Data {
    public class ProjectUpdateData {
        public ProjectUpdateData(Project project, IList<DataPoint> dataPoints) {
            this.Project = project;
            this.DataPoints = dataPoints;
        }

        public Project Project { get; set; }
        public int TotalPoints { 
            get { return Project.AmountEffotrNeeded();  } 
        }
        
        private IList<DataPoint> dataPoints;
        public IList<DataPoint> DataPoints {
            get { return dataPoints; }
            set {
                AddEntryDataPointIfNeeded(value);
                dataPoints = value;                
            }
        }

        private void AddEntryDataPointIfNeeded(IList<DataPoint> value) {
            //if (value.Count > 0) {
            //    value.Insert(0, new DataPoint(this.Project.CreatedAt.Date, 0, this.Project.Tasks.Count()));
            //}
        }

    } //class

    public class DataPoint {
        public DataPoint(DateTime date, int progress, int totalTasksInDay) {
            this.Date = date;
            this.Progress = progress;
            this.TotalPointsInTheDay = totalTasksInDay * 100;
        }

        public DateTime Date { get; set; }
        public int Progress { get; set; }
        public int TotalPointsInTheDay { get; set; }
    }
}
