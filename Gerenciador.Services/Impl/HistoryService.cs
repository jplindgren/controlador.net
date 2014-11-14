using Gerenciador.Domain.Snapshot;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Repository.EntityFramwork.Interface;
using Gerenciador.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Services.Impl {
    public class HistoryService {
        private EventSnapshotRepository eventSnapshotRepository;
        private ITaskProgressHistoryRepository taskProgressRepository;

        public HistoryService(EventSnapshotRepository eventSnapshotRepository, ITaskProgressHistoryRepository taskProgressRepository) {
            this.eventSnapshotRepository = eventSnapshotRepository;
            this.taskProgressRepository = taskProgressRepository;
        }
        public void CreateEntry(EventSnapshot snapshot) {
            eventSnapshotRepository.Add(snapshot);
        }
        public IEnumerable<EventSnapshot> GetByTask(Guid taskId) {
            return eventSnapshotRepository.GetByTaskId(taskId);
        }

        public IList<DataPoint> GetProgressData(Guid projectId) {
            var progressHistory = taskProgressRepository.GetProgressHistory(projectId);
            var result = progressHistory.GroupBy(x => new DateTime(x.UpdatedAt.Year, x.UpdatedAt.Month, x.UpdatedAt.Day))
                .Select(x => new DataPoint(x.Key, x.Sum(y => y.Progress))).ToList();

            //var result = progressHistory.GroupBy(x => new DateTime(x.UpdatedAt.Year, x.UpdatedAt.Month, x.UpdatedAt.Day))
            //    .Select(x => 
            //        new ProjectUpdatePoint(
            //            x.Key, 
            //            progressHistory
            //                .Where(z => new DateTime(z.UpdatedAt.Year, z.UpdatedAt.Month, z.UpdatedAt.Day) <= x.Key)
            //                .Sum(y => y.Progress)
            //        )
            //    ).ToList();
            return result;
        }

    } //class
}
