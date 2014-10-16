using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Web.UI.Controllers{
    [Authorize]
    public class TaskController : Controller{

        private ProjectService _projectService;
        private IDataContext _dataContext;

        public TaskController() {
            _dataContext = new ProjectManagementContext();
            _projectService = new ProjectService(new ProjectRepository(_dataContext));
        }

        //
        // GET: /Task/Create
        public ActionResult Details(Guid projectId, Guid id) {
            var project = _projectService.GetProject(projectId);
            var task = project.Tasks.Where(x => x.Id == id).FirstOrDefault();
            return View(task);
        }

         //
        // GET: /Task/Create
        public ActionResult Create(string id){
            ViewBag.ProjectId = Guid.Parse(id);

            return View();
        }

        //
        // POST: /Task/Create
        [HttpPost]
        public ActionResult Create(Task task){
            try{
                var project = _projectService.GetProject(task.ProjectId);
                project.AddTask(task.Name, task.Description);
                _dataContext.SaveChanges();
                // TODO: Add insert logic here
                return RedirectToAction("Index","Home");
            }catch{
                ViewBag.ProjectId = task.ProjectId;
                return View(task);
            }
        }

    } //class
}
