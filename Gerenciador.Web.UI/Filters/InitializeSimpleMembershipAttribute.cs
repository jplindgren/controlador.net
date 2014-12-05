using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using System.Linq;
using WebMatrix.WebData;
using Gerenciador.Web.UI.Models;
using Gerenciador.Repository.EntityFramwork;
using System.Configuration;
using System.Web.Security;

namespace Gerenciador.Web.UI.Filters {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer {
            public SimpleMembershipInitializer() {
                if (!WebSecurity.Initialized)
                    WebSecurity.InitializeDatabaseConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].Name,
                                                                "UserProfile",
                                                                "UserId",
                                                                "UserName",
                                                                autoCreateTables: true);
            }
        }//inner class
    }  //outer class
}
