namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdkbjv4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EngineerId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "EngineerId");
            AddForeignKey("dbo.AspNetUsers", "EngineerId", "dbo.Engineers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "EngineerId", "dbo.Engineers");
            DropIndex("dbo.AspNetUsers", new[] { "EngineerId" });
            DropColumn("dbo.AspNetUsers", "EngineerId");
        }
    }
}
