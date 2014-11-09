using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork.Impl {
    public class TaskProgressHistoryRepository : Repository<TaskProgressHistory>, ITaskProgressHistoryRepository {
        public TaskProgressHistoryRepository(IDataContext dataContext)
            : base(dataContext) {
            _dataContext = dataContext;
        }

        public IList<TaskProgressHistory> Test(Guid projectId) { 
            var test = GetAll()
                .Where(x => x.ProjectId == projectId && x.Task.Status != Domain.TaskStatus.Cancelled)
                .OrderBy(x => x.UpdatedAt)
                .GroupBy(x => DbFunctions.TruncateTime(x.UpdatedAt))
                .Select(x => new { Date = x.Key, Progress = x.Sum(y => y.Progress) })
                .ToList();
            return null; ;
        }
    }//class
}
