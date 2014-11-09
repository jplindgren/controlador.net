namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectColumnToTaskProgressHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskProgressHistory", "Project_Id", c => c.Guid());
            CreateIndex("dbo.TaskProgressHistory", "Project_Id");
            AddForeignKey("dbo.TaskProgressHistory", "Project_Id", "dbo.Project", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskProgressHistory", "Project_Id", "dbo.Project");
            DropIndex("dbo.TaskProgressHistory", new[] { "Project_Id" });
            DropColumn("dbo.TaskProgressHistory", "Project_Id");
        }
    }
}
