using Gerenciador.Domain.Todo;
using Gerenciador.Domain.UserContext;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Services.Impl {
    public class UserService {
        private IUserProfileRepository userProfileRepository;
        public UserService(IUserProfileRepository userProfileRepository) {
            this.userProfileRepository = userProfileRepository;
        }
        public UserProfile GetUser(string username) {
            return this.userProfileRepository.GetUser(username);
        }

        public IList<UserProfile> GetAllUsers() {
            return this.userProfileRepository.GetAll().ToList();
        }

        public void CreateUser(string username) {
            userProfileRepository.Add(new UserProfile { UserName = username });
        }

        public void RemoveTodoItem(TodoItem item) {
            userProfileRepository.RemoveTodoItem(item);
        }

        public async Task<IList<UserProfile>> GetAllUsersAsync() {
            return await this.userProfileRepository.GetAllAsync();
        }
    } //class
}
