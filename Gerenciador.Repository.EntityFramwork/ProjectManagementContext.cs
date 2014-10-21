using Gerenciador.Domain;
using Gerenciador.Domain.Snapshot;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Repository.EntityFramwork {
    public class ProjectManagementContext : DbContext, IDataContext {
        public ProjectManagementContext() : base("DefaultConnection") { }

        //sets
        public IDbSet<EntityTest> EntitiesTest { get; set; }
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<Gerenciador.Domain.Task> Tasks { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<EventSnapshot> Snapshots { get; set; }
        public IDbSet<SubTask> SubTasks { get; set; }

        public System.Data.Entity.IDbSet<T> Set<T>() where T : class {
            return base.Set<T>();
        }

        public void ExecuteCommand(string command, params object[] parameters) {
            base.Database.ExecuteSqlCommand(command, parameters);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder){
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    } //class
}
