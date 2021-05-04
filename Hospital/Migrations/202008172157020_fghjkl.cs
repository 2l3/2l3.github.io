namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fghjkl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceRequests", "DeviceName", c => c.String());
            AddColumn("dbo.DeviceRequests", "ComputerNumber", c => c.String());
            AddColumn("dbo.DeviceRequests", "WithNoBarCode", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceRequests", "WithNoBarCode");
            DropColumn("dbo.DeviceRequests", "ComputerNumber");
            DropColumn("dbo.DeviceRequests", "DeviceName");
        }
    }
}
