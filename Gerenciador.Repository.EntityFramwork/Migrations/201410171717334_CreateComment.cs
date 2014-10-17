namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new {
                    Id = c.Guid(nullable: false, identity: true),
                    Content = c.String(),
                    AuthorName = c.String(),
                    CreatedAt = c.DateTime(),
                    TaskId = c.Guid(),
                    ProjectId = c.Guid(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .ForeignKey("dbo.Task", t => t.TaskId)
                .Index(t => t.TaskId)
                .Index(t => t.ProjectId)
                .Index(t => t.CreatedAt);          
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "TaskId", "dbo.Task");
            DropForeignKey("dbo.Comment", "ProjectId", "dbo.Project");
            DropIndex("dbo.Comment", new[] { "ProjectId" });
            DropIndex("dbo.Comment", new[] { "TaskId" });
            DropIndex("dbo.Comment", new[] { "CreatedAt" });
            DropTable("dbo.Comment");
        }
    }
}
