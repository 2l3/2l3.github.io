namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class d : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CenterDevices", "LastModificationDate", c => c.DateTime());
            AddColumn("dbo.CenterDeviceSchedules", "LastCloseYear", c => c.Int(nullable: false));
            AddColumn("dbo.CenterDeviceSchedules", "LastPrintPPMYear", c => c.Int(nullable: false));
            AddColumn("dbo.Devices", "LastModificationDate", c => c.DateTime());
            AddColumn("dbo.DeviceSchedules", "LastCloseYear", c => c.Int(nullable: false));
            AddColumn("dbo.DeviceSchedules", "LastPrintPPMYear", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceSchedules", "LastPrintPPMYear");
            DropColumn("dbo.DeviceSchedules", "LastCloseYear");
            DropColumn("dbo.Devices", "LastModificationDate");
            DropColumn("dbo.CenterDeviceSchedules", "LastPrintPPMYear");
            DropColumn("dbo.CenterDeviceSchedules", "LastCloseYear");
            DropColumn("dbo.CenterDevices", "LastModificationDate");
        }
    }
}
