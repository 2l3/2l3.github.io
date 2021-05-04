namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fghjkl5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeviceRequests", "DeviceId", "dbo.Devices");
            DropIndex("dbo.DeviceRequests", new[] { "DeviceId" });
            AlterColumn("dbo.DeviceRequests", "DeviceId", c => c.Int());
            CreateIndex("dbo.DeviceRequests", "DeviceId");
            AddForeignKey("dbo.DeviceRequests", "DeviceId", "dbo.Devices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceRequests", "DeviceId", "dbo.Devices");
            DropIndex("dbo.DeviceRequests", new[] { "DeviceId" });
            AlterColumn("dbo.DeviceRequests", "DeviceId", c => c.Int(nullable: false));
            CreateIndex("dbo.DeviceRequests", "DeviceId");
            AddForeignKey("dbo.DeviceRequests", "DeviceId", "dbo.Devices", "Id", cascadeDelete: true);
        }
    }
}
