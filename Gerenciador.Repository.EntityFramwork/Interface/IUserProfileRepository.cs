using Gerenciador.Domain.UserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerenciador.Repository.EntityFramwork.Interface {
    public interface IUserProfileRepository : IRepository<UserProfile> {
        UserProfile GetUser(string username);
    } //class
}
