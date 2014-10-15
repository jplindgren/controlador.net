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
    public class HomeController : Controller {
        private ProjectSummaryService _projectSummaryService;
        private IDataContext _dataContext;
        public HomeController() {
            _dataContext = new ProjectManagementContext();
            _projectSummaryService = new ProjectSummaryService(new ProjectRepository(_dataContext));
        }

        [Authorize]
        public ActionResult Index() {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            var projectSummary = _projectSummaryService.GetProjectSummary(Guid.Parse("dc3b44fa-1b54-e411-bed8-c48508d65d66"));
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
                new Task() {
                    CreatedAt = DateTime.Now,
                    Description = "Create a view to display everything about a specific project",
                    Done = false,
                    LastUpdatedAt = DateTime.Now,
                    Name = "Create project summary",
                    Progress = 0,
                    Project = project
                },
                new Task() {
                    CreatedAt = DateTime.Now,
                    Description = "Build a method to create a fake project to populate a project summary",
                    Done = false,
                    LastUpdatedAt = DateTime.Now,
                    Name = "Create a fake project",
                    Progress = 50,
                    Project = project
                },
            });
            _projectSummaryService.CreateProject(project);
            _dataContext.SaveChanges();
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
