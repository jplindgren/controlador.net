namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TurnUserIdIntoTodoItemToNullable : DbMigration{
        public override void Up(){
            AlterColumn("dbo.TodoItem", "UserId", c => c.Int(nullable: true));
        }
        
        public override void Down(){
            AlterColumn("dbo.TodoItem", "UserId", c => c.Int(nullable: false));
        }
    }// class
}
