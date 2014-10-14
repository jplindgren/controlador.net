using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Web.UI.Controllers {
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
            var project = _projectSummaryService.GetProjectSummary(Guid.NewGuid());
            return View(project);
        }

        public ActionResult About() {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
