namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndextToTaskStatus : DbMigration
    {
        public override void Up(){
            CreateIndex("dbo.Task", "Status");
        }
        
        public override void Down(){
            CreateIndex("dbo.Task", "Status");
        }
    } //class
}
