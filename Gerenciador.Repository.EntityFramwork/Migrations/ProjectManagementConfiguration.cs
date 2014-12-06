namespace Gerenciador.Repository.EntityFramwork.Migrations{
    using Gerenciador.Domain;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class ProjectManagementConfiguration : DbMigrationsConfiguration<Gerenciador.Repository.EntityFramwork.ProjectManagementContext>{
        public ProjectManagementConfiguration(){
            AutomaticMigrationsEnabled = false;
            ContextKey = "Gerenciador.Repository.EntityFramwork.ProjectManagementContext";
        }

        protected override void Seed(Gerenciador.Repository.EntityFramwork.ProjectManagementContext context){
            var tests = new List<EntityTest>{
                new EntityTest{ Name = "Test 1",  },
                new EntityTest{ Name = "Test 2",  }
            };

            tests.ForEach(s => context.EntitiesTest.Add(s));
            context.SaveChanges();
        }
    } //class
} 
