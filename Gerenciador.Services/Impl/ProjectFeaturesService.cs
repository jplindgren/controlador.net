using Gerenciador.Domain;
using Gerenciador.Repository.EntityFramwork.Impl;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Services.Impl {
    public class ProjectFeaturesService {
        private ICommentRepository commentRepository;
        private IProjectRepository projectRepository;

        public ProjectFeaturesService(IProjectRepository projectRepository, ICommentRepository commentRepository ) {
            this.commentRepository = commentRepository;
            this.projectRepository = projectRepository;
        }

        public IEnumerable<Comment> GetComments(Guid projectId) {
            return this.commentRepository.GetByProjectId(projectId);
        }

        public IEnumerable<Comment> GetComments(Guid projectId, Guid taskId) {
            var project = this.projectRepository.Get(projectId);
            var task = project.Tasks.Where(x => x.Id == taskId).First();
            return this.commentRepository.GetByTask(task);
        }

        public void CreateComment(string content, string username, Guid projectId, Guid? taskId) {
            var comment = new Comment() {
                AuthorName = username,
                Content = content,
                CreatedAt = DateTime.Now,
                ProjectId = projectId,
                TaskId = taskId
            };
            this.commentRepository.Add(comment);
        }
    } //class
}
