using Gerenciador.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerenciador.Repository.EntityFramwork.Interface {
    public interface ICommentRepository : IRepository<Comment> {
        IEnumerable<Comment> GetByProjectId(Guid Id);
        IEnumerable<Comment> GetByTask(Guid projectId, Guid taskId);
    } //class
}
