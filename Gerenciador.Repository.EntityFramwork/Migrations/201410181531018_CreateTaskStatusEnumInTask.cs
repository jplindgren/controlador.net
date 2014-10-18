namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTaskStatusEnumInTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Task", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Task", "Status");
        }
    }
}
