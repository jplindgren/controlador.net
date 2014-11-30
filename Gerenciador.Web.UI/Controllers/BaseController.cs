using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Services.Impl;
using Gerenciador.Web.UI.Filters;
using Gerenciador.Web.UI.Models;
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

        private UserService _userService;
        protected UserService UserService {
            get { return _userService; }
        }

        protected UserProfile CustomUser { 
            get { return _userService.GetUser(User.Identity.Name); } 
        }

        private PageMetadataViewModel _pageMetadataViewModel;
        protected PageMetadataViewModel PageMetadataViewModel {
            get {
                if (_pageMetadataViewModel == null)
                    _pageMetadataViewModel = new PageMetadataViewModel(
                            CustomUser,
                            ""
                        );
                    return _pageMetadataViewModel;
            }
        }

        //protected abstract string GetPageTitle();

        public BaseController(IDataContext context, UserService userService) {
            _dataContext = context;
            _userService = userService;
        }

        public JsonResult CustomJson(object data) {
            JsonNetResult jsonNetResult = new JsonNetResult();
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = data;
            return jsonNetResult;
        }
    } //class
}