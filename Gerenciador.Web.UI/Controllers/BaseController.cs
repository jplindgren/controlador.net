using Gerenciador.Repository.EntityFramwork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Web.UI.Controllers {
    public class BaseController : Controller{
        private IDataContext _dataContext;
        protected IDataContext DataContext {
            get { return _dataContext; }
        }

        public BaseController() {
            _dataContext = new ProjectManagementContext();
        }

        public JsonResult CustomJson(object data) {
            JsonNetResult jsonNetResult = new JsonNetResult();
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = data;
            return jsonNetResult;
        }
    } //class
}