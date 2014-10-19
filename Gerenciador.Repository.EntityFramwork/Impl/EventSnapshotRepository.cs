using Gerenciador.Domain.Snapshot;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork.Impl {
    public class EventSnapshotRepository : Repository<EventSnapshot>, IEventSnapshotRepository {
        public EventSnapshotRepository(IDataContext dataContext)
            : base(dataContext) {
            _dataContext = dataContext;
        }

        public IQueryable<EventSnapshot> GetAllOrdered() {
            return GetAll().OrderByDescending(x => x.EventDate);
        }

        public IEnumerable<EventSnapshot> GetByProjectId(Guid Id) {
            return GetAllOrdered().Where(x => x.ProjectId == Id).AsEnumerable();
        }

        public IEnumerable<EventSnapshot> GetByTaskId(Guid Id) {
            return GetAllOrdered().Where(x => x.TaskId == Id).AsEnumerable();
        }

    }//class
}
