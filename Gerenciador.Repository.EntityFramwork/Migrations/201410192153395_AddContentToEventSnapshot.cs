namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContentToEventSnapshot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventSnapshot", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventSnapshot", "Content");
        }
    }
}
