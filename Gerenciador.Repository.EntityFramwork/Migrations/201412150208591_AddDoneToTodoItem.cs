namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDoneToTodoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodoItem", "Done", c => c.Boolean(nullable: false));
            CreateIndex("dbo.TodoItem", "Done");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TodoItem", "Done");
            DropColumn("dbo.TodoItem", "Done");            
        }
    }
}
