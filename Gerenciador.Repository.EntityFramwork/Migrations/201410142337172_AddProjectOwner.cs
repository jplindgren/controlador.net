namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "Owner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "Owner");
        }
    }
}
