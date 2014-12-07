namespace Gerenciador.Repository.EntityFramwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WhatMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Project", "Owner", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Project", "Owner", c => c.String(nullable: false));
        }
    }
}
