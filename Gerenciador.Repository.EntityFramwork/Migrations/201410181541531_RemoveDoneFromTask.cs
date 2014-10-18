namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDoneFromTask : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Task", "Done");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Task", "Done", c => c.Boolean(nullable: false));
        }
    }
}
