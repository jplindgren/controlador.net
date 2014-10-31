namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransationalItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Task", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Task", "Deadline", c => c.DateTime(nullable: false));
            AddColumn("dbo.Task", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Task", "EndDate");
            DropColumn("dbo.Task", "Deadline");
            DropColumn("dbo.Task", "StartDate");
        }
    }
}
