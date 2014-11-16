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

        public HistoryService(EventSnapshotRepository eventSnapshotRepository) {
            this.eventSnapshotRepository = eventSnapshotRepository;
        }
        public void CreateEntry(EventSnapshot snapshot) {
            eventSnapshotRepository.Add(snapshot);
        }
        public IEnumerable<EventSnapshot> GetByTask(Guid taskId) {
            return eventSnapshotRepository.GetByTaskId(taskId);
        }

    } //class
}
