using Gerenciador.Domain;
using Gerenciador.Domain.Snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerenciador.Repository.EntityFramwork.Interface {
    public interface IEventSnapshotRepository : IRepository<EventSnapshot> {
        IEnumerable<EventSnapshot> GetByProjectId(Guid Id);
        IEnumerable<EventSnapshot> GetByTaskId(Guid Id);
    } //class
}
