namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sjdhvb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CenterDeviceRequests", "ReportDate", c => c.DateTime());
            AddColumn("dbo.DeviceRequests", "ReportDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceRequests", "ReportDate");
            DropColumn("dbo.CenterDeviceRequests", "ReportDate");
        }
    }
}
