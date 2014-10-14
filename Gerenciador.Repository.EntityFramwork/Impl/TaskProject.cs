using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork.Impl {
    public class TaskProject : Repository<Gerenciador.Domain.Task>, ITaskRepository {
        public TaskProject(IDataContext dataContext)
            : base(dataContext) {
            _dataContext = dataContext;
        }
        
    } //class
}
