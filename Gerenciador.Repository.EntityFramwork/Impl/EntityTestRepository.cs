using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork.Impl {
    public class EntityTestRepository : Repository<EntityTest>, IRepository<EntityTest> {
        public EntityTestRepository(IDataContext dataContext)
            : base(dataContext) {
            _dataContext = dataContext;
        }

        public int GetCountById(Guid id) {
            return _dataContext.Set<EntityTest>().Count(x => x.Id == id);
        }

    } //class
}
