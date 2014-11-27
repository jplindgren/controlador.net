﻿using Gerenciador.Domain;
using Gerenciador.Domain.Snapshot;
using Gerenciador.Repository.EntityFramwork.Interface;
using Gerenciador.Services.Hangfire;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerenciador.Services.Impl {
    public class ProjectService {
        private IProjectRepository _projectRepository;
        private HistoryService _historyService;

        public ProjectService(IProjectRepository projectRepository, HistoryService historyService) {
            _projectRepository = projectRepository;
            _historyService = historyService;
        }

        public Project GetProject(Guid id) {
            return _projectRepository.Get(id);
        }

        public void CreateTask(Project project, string username, string taskName, string taskDescription, RangeDate rangeDate) {
            var task = project.AddTask(taskName, taskDescription, rangeDate, username);
            _projectRepository.SaveChanges();

            var snapshot = new EventSnapshotBuilder().ForAction("Create").Consume(task).Create();
            _historyService.CreateEntry(snapshot);
        }

        public void UpdateTask(Task task, int progress, string username) {
            var valueUpdated = progress - task.Progress;

            task.UpdateProgress(progress, username);

            var snapshot = new EventSnapshotBuilder().ForAction("UpdateProgress").Consume(task).Create();
            _historyService.CreateEntry(snapshot);

            //DelayedJobs.Execute(() => CreateProgressHistoryFromThatTask(task.ProjectId, task.Id, valueUpdated, DateTime.Now));
            //BackgroundJob.Enqueue<ProjectService>(x => x.CreateProgressHistoryFromThatTask(task.ProjectId, task.Id, valueUpdated, DateTime.Now));
        }

        public void CreateProgressHistoryFromThatTask(Guid projetId, Guid taskId, int valueUpdated, DateTime today){
            var project = _projectRepository.Get(projetId);
            var task = project.Tasks.Where(x => x.Id == taskId).FirstOrDefault();
            task.CreateProgressHistoryFromThatTask(valueUpdated, today);
            _projectRepository.SaveChanges();
        }

        public void CreateSubTask(Task task, SubTask subtask, string username) {
            task.SubTasks.Add(subtask);
            _projectRepository.SaveChanges();

            var snapshot = new EventSnapshotBuilder().ForAction("Create").ForUser(username).Consume(subtask).Create();
            _historyService.CreateEntry(snapshot);
        }
    } //class
}
