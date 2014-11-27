namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedByAndLastUpdatedByToTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Task", "CreatedBy", c => c.String(nullable: false));
            AddColumn("dbo.Task", "LastUpdatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Task", "LastUpdatedBy");
            DropColumn("dbo.Task", "CreatedBy");
        }
    }
}
