namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jhdsvg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CenterDeviceRequests", "Notes", c => c.String());
            AddColumn("dbo.CenterDeviceRequests", "EngineerId", c => c.Int());
            AddColumn("dbo.DeviceRequests", "Notes", c => c.String());
            AddColumn("dbo.DeviceRequests", "EngineerId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceRequests", "EngineerId");
            DropColumn("dbo.DeviceRequests", "Notes");
            DropColumn("dbo.CenterDeviceRequests", "EngineerId");
            DropColumn("dbo.CenterDeviceRequests", "Notes");
        }
    }
}
