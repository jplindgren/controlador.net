using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Services.Impl;
using Gerenciador.Web.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Web.UI.Controllers{
    public class CommentsController : BaseController{
        private ProjectFeaturesService _projectFeaturesService;
        public CommentsController(IDataContext context, ProjectFeaturesService projectFeatureService, UserService userService)
            : base(context, userService) {
            _projectFeaturesService = projectFeatureService;
	    }

        // GET: /Comments/1
        public PartialViewResult Index(Guid projectId, Guid? taskId) {
            IEnumerable<Comment> comments;
            if (taskId.HasValue)
                comments = _projectFeaturesService.GetComments(projectId, taskId.Value);
            else
                comments = _projectFeaturesService.GetComments(projectId);

            return PartialView("Widgets/_CommentListWidget", comments);
        }

        [HttpPost]
        public JsonResult Create(Guid projectId, Guid? taskId, string content) {
            _projectFeaturesService.CreateComment(content, User.Identity.Name, projectId, taskId);
            DataContext.SaveChanges();
            return Json(new object());
        }
    } //class
}
