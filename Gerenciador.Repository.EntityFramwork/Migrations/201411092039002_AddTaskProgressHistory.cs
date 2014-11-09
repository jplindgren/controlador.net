namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTaskProgressHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskProgressHistory",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Progress = c.Int(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Task_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Task", t => t.Task_Id)
                .Index(t => t.Task_Id)
                .Index(t => t.UpdatedAt);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskProgressHistory", "Task_Id", "dbo.Task");
            DropIndex("dbo.TaskProgressHistory", new[] { "Task_Id" });
            DropIndex("dbo.TaskProgressHistory", new[] { "UpdatedAt" });
            DropTable("dbo.TaskProgressHistory");
        }
    }
}
