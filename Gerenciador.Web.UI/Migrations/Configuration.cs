namespace Gerenciador.Web.UI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<Gerenciador.Web.UI.Models.UsersContext>{
        public Configuration(){
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Gerenciador.Web.UI.Models.UsersContext context){
            WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                                                    "UserProfile",
                                                    "UserId",
                                                    "UserName", autoCreateTables: true);
 
            if (!Roles.RoleExists("Administrator"))
                Roles.CreateRole("Administrator");
 
            if (!WebSecurity.UserExists("joaopozo@gmail.com"))
                WebSecurity.CreateUserAndAccount(
                    "joaopozo@gmail.com",
                    "123456",
                    new { Name = "João Paulo Lindgren", CreatedAt = DateTime.Now });

            if (!Roles.GetRolesForUser("joaopozo@gmail.com").Contains("Administrator"))
                Roles.AddUsersToRoles(new[] { "joaopozo@gmail.com" }, new[] { "Administrator" });
        }
    } //class
}
