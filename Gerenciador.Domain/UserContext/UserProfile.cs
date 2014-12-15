using Gerenciador.Domain.Todo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain.UserContext {
    [Table("UserProfile")]
    public class UserProfile {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<TodoItem> TodoItems { get; set; }
        public TodoItem AddTodoItem(string content, int order) {
            if (TodoItems == null)
                TodoItems = new List<TodoItem>();
            var todoItem = new TodoItem() { Content = content, Order = order, CreatedAt = DateTime.Now, UserProfile = this, Done = false };
            TodoItems.Add(todoItem);
            return todoItem;
        }
    }//class
}
