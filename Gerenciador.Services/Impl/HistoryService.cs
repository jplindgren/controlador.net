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

        public IList<ProjectUpdateHistory> GetProgressData(Guid projectId) {
            DateTime[] dates = new DateTime[3];
            dates[0] = new DateTime(2014, 11, 6);
            dates[1] = new DateTime(2014, 11, 7);
            dates[2] = new DateTime(2014, 11, 9);

            taskProgressRepository.Test(projectId);
            return null;
        }

    } //class
}
