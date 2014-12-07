namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusToProjectAndRequiredFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "Status", c => c.Int(nullable: false, defaultValue: 1));
            AlterColumn("dbo.Project", "Name", c => c.String(nullable: false, defaultValue: "Sample Project"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Project", "Owner", c => c.String());
            AlterColumn("dbo.Project", "Name", c => c.String());
            DropColumn("dbo.Project", "Status");
        }
    }
}
