using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerenciador.Repository.EntityFramwork.Impl {
    public class CommentRepository : Repository<Comment>, ICommentRepository {
        public CommentRepository(IDataContext dataContext)
            : base(dataContext) {
            _dataContext = dataContext;
        }

        public IQueryable<Comment> GetAllOrdered() {
            return GetAll().OrderByDescending(x => x.CreatedAt);
        }

        public IEnumerable<Comment> GetByProjectId(Guid Id) {
            return GetAllOrdered().Where(x => x.ProjectId.HasValue && x.ProjectId == Id).AsEnumerable();
        }
        
        public IEnumerable<Comment> GetByTask(Guid projectId, Guid taskId) {
            return GetAllOrdered().Where(x => x.TaskId == taskId && x.ProjectId.HasValue && x.ProjectId == projectId).AsEnumerable();
        }
    } //class
}
