namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTodoItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TodoItem",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Content = c.String(nullable: false, maxLength: 256),
                        CreatedAt = c.DateTime(nullable: false),
                        Order = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TodoItem", "UserId", "dbo.UserProfile");
            DropIndex("dbo.TodoItem", new[] { "UserId" });
            DropTable("dbo.TodoItem");
        }
    }
}
