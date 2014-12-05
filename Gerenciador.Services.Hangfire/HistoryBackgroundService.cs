using Autofac;
using Gerenciador.Domain.Snapshot;
using Gerenciador.Repository.EntityFramwork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Services.Hangfire {
    public class HistoryBackgroundService {
        ILifetimeScope lifetimeScope;
        public HistoryBackgroundService(ILifetimeScope lifetimeScope) {
            this.lifetimeScope = lifetimeScope;
        }

        public void CreateEntry(EventSnapshot snapshot) {
            using (var scope = lifetimeScope.BeginLifetimeScope("AutofacWebRequest")) {
                var eventSnapshotRepository = scope.Resolve<IEventSnapshotRepository>();
                eventSnapshotRepository.Add(snapshot);
                eventSnapshotRepository.SaveChanges();
            }
        }
    }//class
}
