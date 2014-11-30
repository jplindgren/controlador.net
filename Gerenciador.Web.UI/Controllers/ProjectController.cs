using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Web.UI.Controllers{
    [HandleError]
    public class ProjectController : BaseController{
       private ProjectSummaryService _projectSummaryService;
       public ProjectController(IDataContext context, ProjectSummaryService projectSummaryService, UserService userService)
           : base(context, userService) {
            _projectSummaryService = projectSummaryService;
        }


    } //class
}
