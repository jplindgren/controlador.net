namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEndDateFromTaskToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Task", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Task", "EndDate", c => c.DateTime(nullable: false));
        }
    }
}
