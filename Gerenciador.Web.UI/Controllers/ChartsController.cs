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
        public ChartsController() {
            var historyService = new HistoryService(new EventSnapshotRepository(DataContext), new TaskProgressHistoryRepository(DataContext));
            _projectSummaryService = new ProjectSummaryService(new ProjectRepository(DataContext), historyService);
            _projectService = new ProjectService(new ProjectRepository(DataContext), historyService);
        }

        //
        // GET: /Charts/ProjectBurnDown
        public JsonResult ProjectBurnDownData(Guid projectId){
            var result = _projectSummaryService.GetProgressData(projectId);

            return CustomJson(result);
        }
    } //class
}
