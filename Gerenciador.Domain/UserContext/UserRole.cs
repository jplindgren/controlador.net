using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain.UserContext {
    public class UserRole {
        public static string Administrator {
            get { return UserRoleEnum.Administrator.ToString(); } 
        }

        public static string Regular {
            get { return UserRoleEnum.Regular.ToString(); }
        }

        public static string[] GetRoles() {
            return Enum.GetNames(typeof(UserRoleEnum));
        }

        private enum UserRoleEnum {
            Regular = 0,
            Administrator = 1
        }
    } //class    
}
