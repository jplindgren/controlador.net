﻿using Autofac;
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
using System.Data.Entity;
using WebMatrix.WebData;
using System.Data.Entity.Infrastructure;
using Gerenciador.Web.UI.Models;
using System.Threading;
using Gerenciador.Web.UI.DI.Autofac.Modules;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Xml;
using System.Web.Hosting;
using MvcSiteMapProvider.Web.Mvc;
using System.Configuration;
using System.Web.Security;
using System.Data.Entity.Migrations;
using Gerenciador.Domain.UserContext;
using StackExchange.Profiling;

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

            if (bool.Parse(ConfigurationManager.AppSettings["MigrateDatabaseToLatestVersion"])) {
                var configuration = new Gerenciador.Repository.EntityFramwork.Migrations.ProjectManagementConfiguration();
                var migrator = new DbMigrator(configuration);
                migrator.Update();
            }
            Seed();

            //DbInterception.Add(new DbChaosMonkey());
            DbInterception.Add(new InterceptorLogging());
        }

        protected void Application_BeginRequest() {
            if (Request.IsLocal) {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest() {
            //stop as early as you can, even earlier with MvcMiniProfiler.MiniProfiler.Stop(discardResults: true);
            MiniProfiler.Stop(); 
        }

        private void AutoFacConfig() {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register modules
            builder.RegisterModule(new MvcSiteMapProviderModule()); // Required
            builder.RegisterModule(new MvcModule()); // Required by MVC. Typically already part of your setup (double check the contents of the module).


            builder.RegisterType<UserService>();
            builder.RegisterType<ProjectManagementContext>().As<IDataContext>().InstancePerDependency();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Gerenciador.Services.Impl.ProjectSummaryService)))
                   .Where(t => t.Name.EndsWith("Service")).InstancePerDependency();

            //builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Gerenciador.Services.Hangfire.HistoryBackgroundService)))
            //       .Where(t => t.Name.EndsWith("Service")).InstancePerLifetimeScope();
            builder.RegisterType<Gerenciador.Services.Hangfire.HistoryBackgroundService>().InstancePerDependency();

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


            //// OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //Needed to inject in hangfire
            JobActivator.Current = new AutofacJobActivator(container);

            //MvcSiteMapProvider Section
            // Setup global sitemap loader (required)
            MvcSiteMapProvider.SiteMaps.Loader = container.Resolve<ISiteMapLoader>();

            // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider (optional)
            var validator = container.Resolve<ISiteMapXmlValidator>();
            validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));

            // Register the Sitemaps routes for search engines (optional)
            XmlSiteMapController.RegisterRoutes(RouteTable.Routes);
        }

        private void Seed() {
            WebSecurity.InitializeDatabaseConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].Name,
                                                                            "UserProfile",
                                                                            "UserId",
                                                                            "UserName",
                                                                            autoCreateTables: true);

            foreach (var roleName in UserRole.GetRoles()) {
                if (!Roles.RoleExists(roleName))
                    Roles.CreateRole(roleName);
            }
 
            if (!WebSecurity.UserExists("joaopozo@gmail.com"))
                WebSecurity.CreateUserAndAccount(
                    "joaopozo@gmail.com",
                    "123456",
                    new { Name = "João Paulo Lindgren" , CreatedAt = DateTime.Now});
 
            if (!Roles.GetRolesForUser("joaopozo@gmail.com").Contains(UserRole.Administrator.ToString()))
                Roles.AddUsersToRoles(new[] { "joaopozo@gmail.com" }, new[] { UserRole.Administrator.ToString() });
        }
    }//class
}