using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Web.UI.Controllers {
    [HandleError]
    public class HomeController : BaseController {
        private ProjectSummaryService _projectSummaryService;
        public HomeController() {
            var historyService = new HistoryService(new EventSnapshotRepository(DataContext));
            _projectSummaryService = new ProjectSummaryService(new ProjectRepository(DataContext), new TaskProgressHistoryRepository(DataContext));
        }

        [Authorize]
        public ActionResult Index() {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            var projectSummary = _projectSummaryService.GetProjectSummary(Guid.Parse("c13e450e-7e54-e411-8278-782bcbbc3811"));
            if (projectSummary == null) {
                throw new Exception("Projeto não encontrado");
            }

            return View(projectSummary);
        }

        [Authorize]
        public ActionResult PopulateProject() {
            if (!User.Identity.IsAuthenticated || string.IsNullOrEmpty(User.Identity.Name)) {
                throw new Exception("User is not defined or authenticaed");
            }
            var project = new Project();
            project.CreatedAt = DateTime.Now;
            project.Description = "Create an web app to manage small remote projects. It should help the freelancer/company comunicates with his/its customers in a fashion and simple way";
            project.LastUpdatedAt = DateTime.Now;
            project.Owner = User.Identity.Name;
            project.Name = "Controlador.net";
            project.Tasks = new List<Task>(new Task[] {
                new Task("Create project summary", "Create a view to display everything about a specific project", project.Id, project, DateTime.Now,DateTime.Now.AddDays(20)),
                new Task("Create a fake project", "Build a method to create a fake project to populate a project summary", project.Id, project, DateTime.Now.AddDays(-3), DateTime.Now.AddDays(2)) {
                    Progress = 50
                },
                new Task("A completed task for you =]", "Just to test if it works", project.Id, project, DateTime.Now,DateTime.Now.AddDays(21)) {
                    Progress = 100,
                    Status = TaskStatus.Completed
                },
            });
            _projectSummaryService.CreateProject(project);
            DataContext.SaveChanges();
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
