using Autofac;
using Gerenciador.Repository.EntityFramwork.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using System.Reflection;
using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Services.Impl;
using Hangfire;
using Gerenciador.Web.UI.Controllers;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Repository.EntityFramwork.Interface;

namespace Gerenciador.Web.UI {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            AutoFacConfig();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //DbInterception.Add(new DbChaosMonkey());
            DbInterception.Add(new InterceptorLogging());
        }

        private void AutoFacConfig() {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //builder.RegisterType<ProjectSummaryService>();
            //builder.RegisterType<ProjectService>();
            //builder.RegisterType<HistoryService>();
            //builder.RegisterType<TaskService>();

            builder.RegisterType<ProjectManagementContext>().As<IDataContext>().InstancePerDependency();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Gerenciador.Services.Impl.ProjectSummaryService)))
                   .Where(t => t.Name.EndsWith("Service")).InstancePerDependency();

            builder.RegisterType<ProjectManagementContext>().As<IDataContext>().InstancePerHttpRequest();
            // Scan entityframework repository an assembly trying to resolve all of them
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Gerenciador.Repository.EntityFramwork.ProjectManagementContext)))
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces().InstancePerDependency();

            builder.RegisterType<MessageDispatcher>().As<IMessageDispatcher>();
            

            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Gerenciador.Services.Impl.ProjectSummaryService)))
                   .Where(t => t.Name.EndsWith("Service"));

            builder.RegisterType<ProjectManagementContext>().As<IDataContext>().InstancePerHttpRequest();
            // Scan entityframework repository an assembly trying to resolve all of them
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Gerenciador.Repository.EntityFramwork.ProjectManagementContext)))
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces();

            //// OPTIONAL: Register model binders that require DI.
            //builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            //builder.RegisterModelBinderProvider();

            //// OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            //// OPTIONAL: Enable property injection in view pages.
            //builder.RegisterSource(new ViewRegistrationSource());

            //// OPTIONAL: Enable property injection into action filters.
            //builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            JobActivator.Current = new AutofacJobActivator(container);
        }
    }//class
}