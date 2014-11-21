namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotNullableToNameAndStatusInTask : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Task", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Task", "Name", c => c.String());
        }
    }
}
