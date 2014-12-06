namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserProfileTable : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.UserProfile",
            //    c => new {
            //        UserId = c.Int(nullable: false, identity: true),
            //        UserName = c.String(nullable: false),
            //        Name = c.String(),
            //        CreatedAt = c.DateTime(nullable: false),
            //    })
            //    .PrimaryKey(t => t.UserId);
            AddColumn("dbo.UserProfile", "Name", c => c.String(maxLength: 100));
            AddColumn("dbo.UserProfile", "CreatedAt", c => c.DateTime(nullable: false));
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.UserProfile");
            DropColumn("dbo.UserProfile", "Name");
            DropColumn("dbo.UserProfile", "CreatedAt");
        }
    }
}
