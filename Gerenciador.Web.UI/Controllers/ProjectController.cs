using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Services.Impl;
using Gerenciador.Web.UI.Models;
using MvcSiteMapProvider.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Web.UI.Controllers{
    [Authorize]
    public class ProjectController : BaseController{
       private ProjectSummaryService _projectSummaryService;
       public ProjectController(IDataContext context, ProjectSummaryService projectSummaryService, UserService userService)
           : base(context, userService) {
            _projectSummaryService = projectSummaryService;
        }

       //[SiteMapTitle("Consolidado do Projeto")]
       //public ActionResult Index() {
       //    ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

       //    var projectSummary = _projectSummaryService.GetProjectSummary(Guid.Parse("c13e450e-7e54-e411-8278-782bcbbc3811"));
       //    //ViewBag.Metadata = base.PageMetadataViewModel;

       //    if (projectSummary == null) {
       //        throw new Exception("Projeto não encontrado");
       //    }

       //    return View(ProjectSummaryViewModel.FromProjectSummary(projectSummary));
       //}

       [SiteMapTitle("ProjectName")]
       public ActionResult Details(Guid projectId) {
           var projectSummary = _projectSummaryService.GetProjectSummary(projectId);

           if (projectSummary == null) {
               throw new Exception("Projeto não encontrado");
           }

           return View("Index", ProjectSummaryViewModel.FromProjectSummary(projectSummary));
       }

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
                new Task("Create project summary", "Create a view to display everything about a specific project", project.Id, project, DateTime.Now,DateTime.Now.AddDays(20), User.Identity.Name),
                new Task("Create a fake project", "Build a method to create a fake project to populate a project summary", project.Id, project, DateTime.Now.AddDays(-3), DateTime.Now.AddDays(2), User.Identity.Name) {
                    Progress = 50
                },
                new Task("A completed task for you =]", "Just to test if it works", project.Id, project, DateTime.Now,DateTime.Now.AddDays(21), User.Identity.Name) {
                    Progress = 100,
                    Status = TaskStatus.Completed
                },
            });
           _projectSummaryService.CreateProject(project);
           DataContext.SaveChanges();
           ViewBag.Message = "Your app description page.";

           return View();
       }
        

    } //class
}
