namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventSnapshot : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventSnapshot",
                c => new {
                    Id = c.Guid(nullable: false, identity: true),
                    EventDate = c.DateTime(nullable: false),
                    Subject = c.String(),
                    Author = c.String(),
                    ProjectId = c.Guid(),
                    TaskId = c.Guid(),
                    ResourceId = c.Guid(),
                    Action = c.String(),
                    Resource = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.TaskId)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EventSnapshot");
            DropIndex("dbo.EventSnapshot", new[] { "ProjectId" });
            DropIndex("dbo.EventSnapshot", new[] { "TaskId" });
        }
    }
}
