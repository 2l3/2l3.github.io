namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ghjkl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "Com", c => c.String());
            AddColumn("dbo.Devices", "Serial", c => c.String());
            AddColumn("dbo.Devices", "Model", c => c.String());
            AddColumn("dbo.Devices", "ComputerNumber", c => c.String());
            AddColumn("dbo.DeviceRequests", "ConfirmType", c => c.Int(nullable: false));
            DropColumn("dbo.DeviceRequests", "IsConfirmed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeviceRequests", "IsConfirmed", c => c.Boolean(nullable: false));
            DropColumn("dbo.DeviceRequests", "ConfirmType");
            DropColumn("dbo.Devices", "ComputerNumber");
            DropColumn("dbo.Devices", "Model");
            DropColumn("dbo.Devices", "Serial");
            DropColumn("dbo.Devices", "Com");
        }
    }
}
