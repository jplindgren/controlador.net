using Gerenciador.Domain;
using Gerenciador.Domain.UserContext;
using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Services.Impl;
using Gerenciador.Web.UI.Filters;
using Gerenciador.Web.UI.Models;
using MvcSiteMapProvider.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
        public ActionResult AdminDashboard() {
            ViewBag.Message = "Painel de Controle";

            //dynamic activeProjectsInfo = new ExpandoObject();
            dynamic activeProjectsInfo = _projectService.GetLastActiveProjects(); 

            AdminDashboardViewModel model = new AdminDashboardViewModel();
            model.NumberActiveProjects = activeProjectsInfo.NumberOfActiveProjects;
            model.LastActivesProjects = activeProjectsInfo.LastActiveProjects;
            model.NextTasks = TaskViewModel.FromTask(_taskService.GetNextTasksForAdmin());

            model.Users = UserService.GetAllUsers();

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

    } //class
}
