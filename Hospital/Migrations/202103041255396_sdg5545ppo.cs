namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdg5545ppo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CenterDevices", "CompanyEmail", c => c.String());
            AddColumn("dbo.CenterDevices", "CompanyPhone", c => c.String());
            AddColumn("dbo.DCenterDevices", "CompanyEmail", c => c.String());
            AddColumn("dbo.DCenterDevices", "CompanyPhone", c => c.String());
            AddColumn("dbo.DDevices", "CompanyEmail", c => c.String());
            AddColumn("dbo.DDevices", "CompanyPhone", c => c.String());
            AddColumn("dbo.DeviceUnits", "CustodyOfficial", c => c.String());
            AddColumn("dbo.DeviceUnits", "CustodyOPhone", c => c.String());
            AddColumn("dbo.Devices", "CompanyEmail", c => c.String());
            AddColumn("dbo.Devices", "CompanyPhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Devices", "CompanyPhone");
            DropColumn("dbo.Devices", "CompanyEmail");
            DropColumn("dbo.DeviceUnits", "CustodyOPhone");
            DropColumn("dbo.DeviceUnits", "CustodyOfficial");
            DropColumn("dbo.DDevices", "CompanyPhone");
            DropColumn("dbo.DDevices", "CompanyEmail");
            DropColumn("dbo.DCenterDevices", "CompanyPhone");
            DropColumn("dbo.DCenterDevices", "CompanyEmail");
            DropColumn("dbo.CenterDevices", "CompanyPhone");
            DropColumn("dbo.CenterDevices", "CompanyEmail");
        }
    }
}
