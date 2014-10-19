namespace Gerenciador.Repository.EntityFramwork.Migrations{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexEventDateInEventSnapshot : DbMigration{
        public override void Up(){
            CreateIndex("dbo.EventSnapshot", "EventDate");
        }
        
        public override void Down(){
            DropIndex("dbo.EventSnapshot", new[] { "EventDate" });
        }
    } //class
}
