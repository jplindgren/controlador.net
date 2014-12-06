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
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Gerenciador.Web.UI.Controllers {
    [Authorize]
    public class HomeController : BaseController {
        private ProjectSummaryService _projectSummaryService;
        public HomeController(IDataContext context, ProjectSummaryService projectSummaryService, UserService userService)
            : base(context, userService) {
            _projectSummaryService = projectSummaryService;
        }

        [SiteMapTitle("Consolidado do Projeto")]
        public RedirectToRouteResult Index() {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            if (Roles.IsUserInRole(UserRole.Administrator)) {
                return RedirectToAction("AdminDashboard");
            } else {
                return RedirectToAction("UserDashboard");
            }
        }


        public ActionResult AdminDashboard() {
            ViewBag.Message = "AdminDashboard";

            return View();
        }

        public ActionResult UserDashboard() {
            ViewBag.Message = "UserDashboard";

            return View();
        }

        public ActionResult Error() {
            return View();
        }
    } //class
}
