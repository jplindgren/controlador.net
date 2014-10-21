namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubTask : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubTask",
                c => new {
                    Id = c.Guid(nullable: false, identity: true),
                    Name = c.String(),
                    CreatedAt = c.DateTime(nullable: false),
                    StartDate = c.DateTime(nullable: false),
                    ExpectedEndDate = c.DateTime(nullable: false),
                    Status = c.Int(nullable: false),
                    TaskId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Task", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId)
                .Index(t => t.Status)
                .Index(t => t.StartDate)
                .Index(t => t.ExpectedEndDate);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubTask", "TaskId", "dbo.Task");
            DropIndex("dbo.SubTask", new[] { "TaskId" });
            DropIndex("dbo.SubTask", new[] { "Status" });
            DropIndex("dbo.SubTask", new[] { "StartDate" });
            DropIndex("dbo.SubTask", new[] { "ExpectedEndDate" });
            DropTable("dbo.SubTask");
        }
    }
}
