using Gerenciador.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Web.UI.Models.CommentsViewModel {
    public class CommentWidgetViewModel {
        public Guid Id { get; set; }

        
        private string content;
        [AllowHtml]
        public string Content {
            get { return content; }
            set { content = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(value, false); }
        }
        public string AuthorName { get; set; }

        public DateTime CreatedAt { get; set; }

        public static CommentWidgetViewModel From(Comment comment) {
            return new CommentWidgetViewModel() {
                Id = comment.Id,
                Content = comment.Content,
                AuthorName = comment.AuthorName,
                CreatedAt = comment.CreatedAt
            };
        }

        public static IList<CommentWidgetViewModel> From(IList<Comment> comments) {
            return comments.Select(comment => From(comment)).ToList();
        }
    } //class
}