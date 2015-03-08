using Gerenciador.Domain.UserContext;
using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Services.Impl;
using Gerenciador.Web.UI.Filters;
using Gerenciador.Web.UI.Models;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using MvcSiteMapProvider.Web.Mvc.Filters;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Gerenciador.Web.UI.Controllers {
    [Authorize]
    public class HomeController : BaseController {
        private ProjectService _projectService;
        private TaskService _taskService;
        private ProjectSummaryService _projectSummaryService;
        public HomeController(IDataContext context, ProjectSummaryService projectSummaryService, UserService userService, 
                                ProjectService projectService, TaskService taskService)
            : base(context, userService) {
            _projectSummaryService = projectSummaryService;
            _projectService = projectService;
            _taskService = taskService;
        }

        [SiteMapTitle("Painel de controle")]
        public RedirectToRouteResult Index() {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            if (Roles.IsUserInRole(UserRole.Administrator)) {
                return RedirectToAction("AdminDashboard");
            } else {
                return RedirectToAction("UserDashboard");
            }
        }

        [SiteMapTitle("Painel de controle")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> AdminDashboard() {
            ViewBag.Message = "Painel de Controle";
            AdminDashboardViewModel model = new AdminDashboardViewModel();
            
            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("GetLastActiveProjects")) {
                dynamic activeProjectsInfo = await _projectService.GetLastActiveProjectsAsync();
                model.NumberActiveProjects = activeProjectsInfo.NumberOfActiveProjects;
                model.LastActivesProjects = activeProjectsInfo.LastActiveProjects;
                model.NextTasks = AdminDashboardViewModel.TaskDashboardViewModel.FromTask(_taskService.GetNextTasksForAdmin());
            }

            using (profiler.Step("GetUsers")) {
                model.Users = await UserService.GetAllUsersAsync();
                model.CurrentUser = model.Users.Where(x => x.UserName == User.Identity.Name).First();
            }

            //Mocks            
            model.NumberOfNewMessages = 4;
            model.NumberOfProposals = 1;
            model.NumberOfTickets = 1;

            return View(model);
        }

        [SiteMapTitle("Painel de controle")]
        public ActionResult UserDashboard() {
            ViewBag.Message = "UserDashboard";

            return View();
        }

        public ActionResult Error() {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Test() {
            return new HttpStatusCodeResult(503);
            //return View();
        }

        private string GetAzureServiceBusConnectionString() {
            return CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
        }

        
        public ActionResult SendAzureBusMessage() {
            ViewBag.Message = "Send Azure bus message.";
            // Create the queue if it does not exist already
            string connectionString = GetAzureServiceBusConnectionString();

            QueueClient client = QueueClient.CreateFromConnectionString(connectionString, "TestQueue");
            for (int i = 5; i < 10; i++) {
                // Create message, passing a string message for the body
                BrokeredMessage message = new BrokeredMessage("Test message " + i);

                // Set some addtional custom app-specific properties
                message.Properties["TestProperty"] = "TestValue";
                message.Properties["Message number"] = i;

                // Send message to the queue
                client.Send(message);
            }

            return RedirectToAction("Index");
        }

        public async Task<string> WaitAsynchronouslyAsync() {
            await Task.Delay(10000);
            return "Finished";
        }
    } //class
}
