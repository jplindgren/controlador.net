using Gerenciador.Domain;
using Gerenciador.Domain.Snapshot;
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
    public class TaskController : BaseController{
        private ProjectService _projectService;
        private HistoryService _historyService;

        public TaskController() {
            _projectService = new ProjectService(new ProjectRepository(DataContext), new HistoryService(new EventSnapshotRepository(DataContext)));
            _historyService = new HistoryService(new EventSnapshotRepository(DataContext));
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
                _projectService.CreateTask(project, User.Identity.Name, task.Name, task.Description);
                DataContext.SaveChanges();
                return RedirectToAction("Index","Home");
            }catch{
                ViewBag.ProjectId = task.ProjectId;
                return View(task);
            }
        }

        //
        // POST: /Task/UpdateProgress
        [HttpPost]
        public JsonResult UpdateProgress(Guid projectId, Guid id, int newValue) {
            var project = _projectService.GetProject(projectId);
            var task = project.Tasks.Where(x => x.Id == id).FirstOrDefault();

            _projectService.UpdateTask(task, newValue, User.Identity.Name);
            DataContext.SaveChanges();
            return Json(task.Progress);
        }

        // POST: /Task/CreateSubTask
        [HttpPost]
        public JsonResult CreateSubTask(Guid projectId, Guid taskId, string name, DateTime startDate, DateTime endDate) {
            var project = _projectService.GetProject(projectId);
            var task = project.Tasks.Where(x => x.Id == taskId).FirstOrDefault();

            SubTask subtask = new SubTask();
            subtask.Name = name;
            subtask.CreatedAt = DateTime.Now;
            subtask.StartDate = startDate;
            subtask.ExpectedEndDate = endDate;
            subtask.Status = TaskStatus.Open;

            task.SubTasks.Add(subtask);

            DataContext.SaveChanges();
            return Json(new { name = subtask.Name, 
                              createdAt = subtask.CreatedAt, 
                              startDate = subtask.StartDate, 
                              expectedEndDate = subtask.ExpectedEndDate, 
                              status = subtask.Status });
        }

        public PartialViewResult GetTimeline(Guid taskId) {
            IEnumerable<EventSnapshot> snapshots;

            snapshots = _historyService.GetByTask(taskId);

            return PartialView("~/Views/Task/_TaskTimelineWidget.cshtml", snapshots.ToList());
        }

    } //class
}
