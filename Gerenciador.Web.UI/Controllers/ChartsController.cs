using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Services.Impl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Web.UI.Controllers{
    [Authorize]
    public class ChartsController : BaseController {
        private ProjectSummaryService _projectSummaryService;
        private ProjectService _projectService;
        public ChartsController(IDataContext context, ProjectSummaryService projectSummaryService, ProjectService projectService, UserService userService)
            : base(context, userService) {
            _projectSummaryService = projectSummaryService;
            _projectService = projectService;
        }

        //
        // GET: /Charts/ProjectBurnDown
        public JsonResult ProjectBurnDownData(Guid projectId){
            var result = _projectSummaryService.GetProgressData(projectId);

            return CustomJson(result);
        }
    } //class
}
