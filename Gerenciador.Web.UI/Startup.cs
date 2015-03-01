using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using System.Configuration;

[assembly: OwinStartup(typeof(Gerenciador.Web.UI.Startup))]

namespace Gerenciador.Web.UI {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            app.UseHangfire(config => {
                // Basic setup required to process background jobs.
                config.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                config.UseServer();

                config.UseAuthorizationFilters(new AuthorizationFilter {
                    //Users = "jplindgren", // allow only specified users
                    Roles = "Administrator" // allow only specified roles
                });

                //config.UseAuthorizationFilters(new ClaimsBasedAuthorizationFilter("hangfire", "access"));
                    
            });
            
        }
    } //class
}