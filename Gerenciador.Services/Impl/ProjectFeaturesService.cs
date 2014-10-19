using Gerenciador.Domain;
using Gerenciador.Domain.Snapshot;
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
        private HistoryService historyService;

        public ProjectFeaturesService(IProjectRepository projectRepository, ICommentRepository commentRepository, HistoryService historyService) {
            this.commentRepository = commentRepository;
            this.projectRepository = projectRepository;
            this.historyService = historyService;
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
            this.commentRepository.SaveChanges();

            var snapshot = new EventSnapshotBuilder().ForAction("Create")
                                                     .Consume(comment)
                                                     .Create();
            this.historyService.CreateEntry(snapshot);

        }


    //    using (var dbContextTransaction = context.Database.BeginTransaction())
    //{
    //    try
    //    {
    //        // do your changes
    //        context.SaveChanges();

    //        // do another changes
    //        context.SaveChanges();

    //        dbContextTransaction.Commit();
    //    }
    //    catch (Exception)
    //    {
    //        dbContextTransaction.Rollback();
    //    }
    //}

    } //class
}
