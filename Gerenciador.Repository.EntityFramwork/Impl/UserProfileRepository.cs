using Gerenciador.Domain.Todo;
using Gerenciador.Domain.UserContext;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork.Impl {
    public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository {
        public UserProfileRepository(IDataContext dataContext)
            : base(dataContext) {}

        public UserProfile GetUser(string username) {
            return GetAll().Where(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefault();
        }

        public void RemoveTodoItem(TodoItem todoItem) {
            base._dataContext.Set<TodoItem>().Remove(todoItem);
        }

        public async Task<IList<UserProfile>> GetAllAsync() {
            return await GetAll().ToListAsync();
        }
    } //class
}
