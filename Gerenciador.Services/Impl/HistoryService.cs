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
        private IEventSnapshotRepository eventSnapshotRepository;

        public HistoryService(IEventSnapshotRepository eventSnapshotRepository) {
            this.eventSnapshotRepository = eventSnapshotRepository;
        }

        public void CreateEntry(EventSnapshot snapshot) {
            this.eventSnapshotRepository.Add(snapshot);
        }
        public IEnumerable<EventSnapshot> GetByTask(Guid taskId) {
            return this.eventSnapshotRepository.GetByTaskId(taskId);
        }

    } //class
}
