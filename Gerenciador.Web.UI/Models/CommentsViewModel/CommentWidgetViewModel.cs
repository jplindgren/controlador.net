using Gerenciador.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Models.CommentsViewModel {
    public class CommentWidgetViewModel {
        public Guid Id { get; set; }
        public string Content { get; set; }
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