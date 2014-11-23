using Autofac;
using Gerenciador.Domain;
using Gerenciador.Domain.Snapshot;
using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Services.Impl;
using Gerenciador.Web.UI.Helpers;
using Gerenciador.Web.UI.Models;
using Gerenciador.Web.UI.Services;
using Hangfire;
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

        public TaskController(IDataContext context, HistoryService historyService, ProjectService projectService, TaskService taskService, UserService userService)
            : base(context, userService) {
            _historyService = historyService;
            _projectService = projectService;
            _taskService = taskService;
        }

        //
        // GET: /Task/Index
        public ActionResult Index(Guid projectId) {
            var project = _projectService.GetProject(projectId);
            return View(TaskViewModel.FromTask(project.Tasks));
        }

        //
        // GET: /Task/Details/taskId
        public ActionResult Details(Guid projectId, Guid id) {
            var project = _projectService.GetProject(projectId);
            var task = project.Tasks.Where(x => x.Id == id).FirstOrDefault();
            return View(TaskViewModel.FromTask(task));
        }

         //
        // GET: /Task/Create
        public ActionResult Create(string projectId){
            ViewBag.ProjectId = Guid.Parse(projectId);

            return View();
        }

        //
        // POST: /Task/Create
        [HttpPost]
        public ActionResult Create(TaskViewModel taskViewModel){
            if (!ModelState.IsValid) {
                ViewBag.ProjectId = taskViewModel.ProjectId;
                return View(taskViewModel);
            }

            try{
                var project = _projectService.GetProject(taskViewModel.ProjectId);
                var rangeDate = new RangeDate(taskViewModel.StartDate, taskViewModel.Deadline);
                _projectService.CreateTask(project, User.Identity.Name, taskViewModel.Name, taskViewModel.Description, rangeDate);
                DataContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }catch {
                return RedirectToAction("Error", "Home");
            }
        }

        //
        // POST: /Task/UpdateProgress
        [HttpPost]
        public JsonResult UpdateProgress(Guid projectId, Guid id, int newValue) {
            var project = _projectService.GetProject(projectId);
            var task = project.Tasks.Where(x => x.Id == id).FirstOrDefault();
            var valueUpdated = newValue - task.Progress;

            _projectService.UpdateTask(task, newValue, User.Identity.Name);
            DataContext.SaveChanges();

            BackgroundJob.Enqueue<IMessageDispatcher>(x => x.OnMessage(task.ProjectId, task.Id, valueUpdated, DateTime.Now));

            return Json(task.Progress);
        }

        public void CreateProgressHistoryFromThatTask(Guid projetId, Guid taskId, int valueUpdated, DateTime today) {
            _projectService.CreateProgressHistoryFromThatTask(projetId, taskId, valueUpdated, today);
            DataContext.SaveChanges();
        }

        [HttpGet]
        public PartialViewResult EditTask(Guid taskId) {
            var task = _taskService.GetTask(taskId);
            return PartialView("~/Views/Task/_Edit.cshtml", task);
        }

        [HttpPost]
        public JsonResult EditTask(FormCollection collection) {
            var task = _taskService.GetTask(Guid.Parse(collection["Id"]));
            TryUpdateModel(task);

            DataContext.SaveChanges();
            var json = JsonConvert.SerializeObject(new TaskViewModel() {
                Id = task.Id,
                Name = task.Name,
                CreatedAt = task.CreatedAt,
                Deadline = task.Deadline,
                Description = task.Description,
                EndDate = task.EndDate,
                LastUpdatedAt = task.LastUpdatedAt,
                Progress = task.Progress,
                ProjectId = task.ProjectId,
                StartDate = task.StartDate,
                Status = task.Status,
                SubTasks = task.GetOrderedSubtasks()
            },
                            Formatting.None,
                            new JsonSerializerSettings() {
                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                            });
            return Json(json);
        }

        [HttpGet]
        public PartialViewResult EditSubTask(Guid taskId, Guid subTaskId) {
            var subktask = _taskService.GetSubTask(taskId, subTaskId);
            return PartialView("~/Views/Task/_EditSubTask.cshtml", subktask);
        }

        [HttpPost]
        public PartialViewResult EditSubTask(FormCollection collection) {
            var subtask = _taskService.GetSubTask(Guid.Parse(collection["TaskId"]), Guid.Parse(collection["Id"]));
            subtask.Name = collection["Name"];
            subtask.StartDate = Convert.ToDateTime(collection["StartDate"]);
            subtask.ExpectedEndDate = Convert.ToDateTime(collection["ExpectedEndDate"]);
            DataContext.SaveChanges();
            return PartialView("~/Views/Task/_SubTaskWidget.cshtml", _taskService.GetSubTasks(subtask.TaskId));
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
        }

        // POST: /Task/CreateSubTask
        [HttpPost]
        public JsonResult CreateSubTaskV2(Guid projectId, Guid taskId, string name, DateTime startDate, DateTime endDate) {
            var project = _projectService.GetProject(projectId);
            var task = project.Tasks.Where(x => x.Id == taskId).FirstOrDefault();

            SubTask subtask = new SubTask(name, startDate, endDate);
            _projectService.CreateSubTask(task, subtask, User.Identity.Name);
            DataContext.SaveChanges();

            return Json(new SubTask() { 
                CreatedAt = subtask.CreatedAt,
                ExpectedEndDate = subtask.ExpectedEndDate,
                Id = subtask.Id,
                Name = subtask.Name,
                StartDate = subtask.StartDate,
                Status = subtask.Status,
                TaskId = subtask.TaskId
            });
        }

        [HttpGet]
        public PartialViewResult GetTimeline(Guid taskId) {
            IEnumerable<EventSnapshot> snapshots;
            snapshots = _historyService.GetByTask(taskId);
            return PartialView("~/Views/Task/_TaskTimelineWidget.cshtml", snapshots.ToList());
        }

        [HttpGet]
        public JsonResult GetLimitDates(Guid projectId, Guid taskId) {
            var project = _projectService.GetProject(projectId);
            var task = project.Tasks.Where(x => x.Id == taskId).FirstOrDefault();
            var limitDates = task.SubTasks.Where(x => x.Status == TaskStatus.Open).Select(x => new LimitDate(x.StartDate, x.ExpectedEndDate, x.Name));

            JsonNetResult jsonNetResult = new JsonNetResult();
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = limitDates;
            return jsonNetResult;
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
    } //class

    public class MessageDispatcher : IMessageDispatcher {
        ILifetimeScope _lifetimeScope;

        public MessageDispatcher(ILifetimeScope lifetimeScope) {
            _lifetimeScope = lifetimeScope;
        }

        public void OnMessage(Guid projetId, Guid taskId, int valueUpdated, DateTime today) {
            using (var messageScope = _lifetimeScope.BeginLifetimeScope("AutofacWebRequest")) {
                var test = messageScope.Resolve<ProjectService>();
                var projectService = messageScope.Resolve<ProjectService>();
                projectService.CreateProgressHistoryFromThatTask(projetId, taskId, valueUpdated, today);
            }
        }
    }//class

    public interface IMessageDispatcher {
        void OnMessage(Guid projetId, Guid taskId, int valueUpdated, DateTime today);
    }
}
