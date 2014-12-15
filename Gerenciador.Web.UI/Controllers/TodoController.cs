using Gerenciador.Domain.Todo;
using Gerenciador.Domain.UserContext;
using Gerenciador.Repository.EntityFramwork;
using Gerenciador.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Gerenciador.Web.UI.Controllers{
    [Authorize()]
    public class TodoController : BaseController{
        public UserService _userService;

        public TodoController(IDataContext dataContext, UserService userService) : base(dataContext, userService) {
            _userService = userService;
        }

        [HttpPost]
        public JsonResult EditItem(Guid id, string content){
            UserProfile userProfile = UserService.GetUser(User.Identity.Name);
            var item = userProfile.TodoItems.Where(x => x.Id == id).First();
            item.Content = content;
            DataContext.SaveChanges();
            return CustomJson(item);
        }

        [HttpPost]
        public JsonResult CreateItem(TodoItem todoItem) {
            UserProfile userProfile = UserService.GetUser(User.Identity.Name);
            var addedItem = userProfile.AddTodoItem(todoItem.Content, todoItem.Order);
            DataContext.SaveChanges();
            return CustomJson(addedItem);
        }

        [HttpPost]
        public JsonResult MarkAsDone(Guid id) {
            UserProfile userProfile = UserService.GetUser(User.Identity.Name);
            var item = userProfile.TodoItems.Where(x => x.Id == id).First();
            item.Done = true;
            
            DataContext.SaveChanges();
            return CustomJson(item);
        }

        [HttpPost]
        public JsonResult DeleteItem(Guid id) {
            UserProfile userProfile = UserService.GetUser(User.Identity.Name);
            var item = userProfile.TodoItems.Where(x => x.Id == id).First();
            
            userProfile.TodoItems.Remove(item);

            DataContext.SaveChanges();
            return CustomJson("");
        }
        
    }//class
}
