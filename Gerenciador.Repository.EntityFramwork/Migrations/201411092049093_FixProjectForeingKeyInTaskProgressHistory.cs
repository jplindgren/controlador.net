namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixProjectForeingKeyInTaskProgressHistory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskProgressHistory", "Task_Id", "dbo.Task");
            DropForeignKey("dbo.TaskProgressHistory", "Project_Id", "dbo.Project");
            DropIndex("dbo.TaskProgressHistory", new[] { "Project_Id" });
            DropIndex("dbo.TaskProgressHistory", new[] { "Task_Id" });
            RenameColumn(table: "dbo.TaskProgressHistory", name: "Task_Id", newName: "TaskId");
            RenameColumn(table: "dbo.TaskProgressHistory", name: "Project_Id", newName: "ProjectId");
            AlterColumn("dbo.TaskProgressHistory", "ProjectId", c => c.Guid(nullable: false));
            AlterColumn("dbo.TaskProgressHistory", "TaskId", c => c.Guid(nullable: false));
            CreateIndex("dbo.TaskProgressHistory", "TaskId");
            CreateIndex("dbo.TaskProgressHistory", "ProjectId");
            AddForeignKey("dbo.TaskProgressHistory", "TaskId", "dbo.Task", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TaskProgressHistory", "ProjectId", "dbo.Project", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskProgressHistory", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.TaskProgressHistory", "TaskId", "dbo.Task");
            DropIndex("dbo.TaskProgressHistory", new[] { "ProjectId" });
            DropIndex("dbo.TaskProgressHistory", new[] { "TaskId" });
            AlterColumn("dbo.TaskProgressHistory", "TaskId", c => c.Guid());
            AlterColumn("dbo.TaskProgressHistory", "ProjectId", c => c.Guid());
            RenameColumn(table: "dbo.TaskProgressHistory", name: "ProjectId", newName: "Project_Id");
            RenameColumn(table: "dbo.TaskProgressHistory", name: "TaskId", newName: "Task_Id");
            CreateIndex("dbo.TaskProgressHistory", "Task_Id");
            CreateIndex("dbo.TaskProgressHistory", "Project_Id");
            AddForeignKey("dbo.TaskProgressHistory", "Project_Id", "dbo.Project", "Id");
            AddForeignKey("dbo.TaskProgressHistory", "Task_Id", "dbo.Task", "Id");
        }
    }
}
