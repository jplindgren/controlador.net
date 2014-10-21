﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Domain.Snapshot {
    public class EventSnapshotBuilder {
        EventSnapshot systemEventSnapshot;

        public EventSnapshotBuilder() {
            systemEventSnapshot = new EventSnapshot();
        }

        public EventSnapshot Create() {
            return systemEventSnapshot;
        }

        public EventSnapshotBuilder ForUser(string username) {
            systemEventSnapshot.Author = username;
            return this;
        }

        public EventSnapshotBuilder ForProject(Guid projectId) {
            systemEventSnapshot.ProjectId = projectId;
            return this;
        }

        public EventSnapshotBuilder ForTask(Guid taskId) {
            systemEventSnapshot.TaskId = taskId;
            return this;
        }

        public EventSnapshotBuilder ForAction(string action) {
            systemEventSnapshot.Action = action;
            return this;
        }

        public EventSnapshotBuilder Consume(Comment comment) {
            systemEventSnapshot.ProjectId = comment.ProjectId;
            systemEventSnapshot.Author = comment.AuthorName;
            systemEventSnapshot.EventDate = comment.CreatedAt;
            systemEventSnapshot.Resource = comment.GetType().AssemblyQualifiedName;
            systemEventSnapshot.ResourceId = comment.Id;
            systemEventSnapshot.Subject = string.Format("{0} enviou um comentário", systemEventSnapshot.Author);
            systemEventSnapshot.Content = comment.Content;
            systemEventSnapshot.TaskId = comment.TaskId;
            return this;
        }

        public EventSnapshotBuilder Consume(Task task) {
            systemEventSnapshot.ProjectId = task.ProjectId;
            systemEventSnapshot.EventDate = task.LastUpdatedAt;
            systemEventSnapshot.Resource = task.GetType().AssemblyQualifiedName;
            systemEventSnapshot.ResourceId = task.Id;

            if (systemEventSnapshot.Action == "Update")
                systemEventSnapshot.Subject = string.Format("{0} atualizou o progresso da task para: {1}", systemEventSnapshot.Author, task.Progress);
            else
                systemEventSnapshot.Subject = string.Format("Task criada por {0}", systemEventSnapshot.Author);
            
            systemEventSnapshot.TaskId = task.Id;
            return this;
        }
    } //class
}