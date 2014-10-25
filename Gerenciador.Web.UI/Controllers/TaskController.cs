using Gerenciador.Domain;
using Gerenciador.Domain.Snapshot;
using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Services.Impl;
using Gerenciador.Web.UI.Helpers;
using Newtonsoft.Json;
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
        private TaskService _taskService;

        public TaskController() {
            _projectService = new ProjectService(new ProjectRepository(DataContext), new HistoryService(new EventSnapshotRepository(DataContext)));
            _historyService = new HistoryService(new EventSnapshotRepository(DataContext));
            _taskService = new TaskService(new TaskRepository(DataContext), new HistoryService(new EventSnapshotRepository(DataContext)));
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

        [HttpPost]
        public PartialViewResult ChangeTaskStatus(Guid taskId, Guid subTaskId, TaskStatus subkTaskStatus) {
            var updatedSubtask = _taskService.ChangeSubTaskStatus(taskId, subTaskId, subkTaskStatus, User.Identity.Name);
            DataContext.SaveChanges();

            return PartialView("~/Views/Task/_SubTaskWidget.cshtml", _taskService.GetSubTasks(taskId));
        }

        // POST: /Task/CreateSubTask
        [HttpPost]
        public PartialViewResult CreateSubTask(Guid projectId, Guid taskId, string name, DateTime startDate, DateTime endDate) {
            var project = _projectService.GetProject(projectId);
            var task = project.Tasks.Where(x => x.Id == taskId).FirstOrDefault();

            SubTask subtask = new SubTask(name, startDate, endDate);
            _projectService.CreateSubTask(task, subtask, User.Identity.Name);
            DataContext.SaveChanges();

            return PartialView("~/Views/Task/_SubTaskWidget.cshtml", _taskService.GetSubTasks(taskId));
            //return Json(new {
            //    id = subtask.Id,
            //    name = subtask.Name,
            //    createdAt = subtask.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss"),
            //    startDate = subtask.StartDate.ToString("dd/MM/yyyy hh:mm:ss"),
            //    expectedEndDate = subtask.ExpectedEndDate.ToString("dd/MM/yyyy hh:mm:ss"),
            //    status = new {
            //        data = TraduzirStatusTeporarioGambiarra(subtask.Status.ToString()),
            //        cssClass = DefineStatusLabelClass(subtask.Status.ToString()),
            //        id = "taskStatus#" + subtask.Id.ToString()
            //    } 
            //});
        }

        //TODO: Remove this shit. Use some AOP in enum
        private static string TraduzirStatusTeporarioGambiarra(string status) {
            if (status == "Open") {
                return "aberta";
            } else if (status == "Completed") {
                return "completa";
            } else if (status == "Cancelled") {
                return "cancelada";
            } else if (status == "InProgress") {
                return "em andamento";
            } else {
                return "desconhecida";
            }
        }

        //TODO: Apply DRY. We have duplicate logic here and on StatusHelper
        private string DefineStatusLabelClass(string status) {
            if (status == "Open") {
                return "label label-warning";
            } else if (status == "Completed") {
                return "label label-success";
            } else if (status == "Cancelled") {
                return "label label-danger";
            } else if (status == "InProgress") {
                return "label label-info";
            } else {
                return "label label-default";
            }
        }

        public PartialViewResult GetTimeline(Guid taskId) {
            IEnumerable<EventSnapshot> snapshots;

            snapshots = _historyService.GetByTask(taskId);

            return PartialView("~/Views/Task/_TaskTimelineWidget.cshtml", snapshots.ToList());
        }

    } //class
}
