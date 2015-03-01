namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexToProjectStatus : DbMigration
    {
        public override void Up(){
            CreateIndex("dbo.Project", "Status");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Project", "Status");
        }
    } //class
}
