using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork.Impl {
    public class TaskRepository : Repository<Gerenciador.Domain.Task>, ITaskRepository {
        public TaskRepository(IDataContext dataContext)
            : base(dataContext) {
        }
        
    } //class
}
