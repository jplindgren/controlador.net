using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Services {
    public class UserService {
        public UserService(UsersContext userContext) {
            this.UserContext = userContext;
        }
        public UserProfile GetUser(string username) {
            return this.UserContext.UserProfiles.Where(x => x.UserName == username).FirstOrDefault();
        }

        public UsersContext UserContext { get; set; }
    } //class
}