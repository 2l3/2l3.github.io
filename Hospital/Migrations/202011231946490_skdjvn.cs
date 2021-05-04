namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class skdjvn : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CenterDevices", "DeviceUnitId", "dbo.DeviceUnits");
            DropIndex("dbo.CenterDevices", new[] { "DeviceUnitId" });
            DropColumn("dbo.CenterDevices", "DeviceUnitId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CenterDevices", "DeviceUnitId", c => c.Int(nullable: false));
            CreateIndex("dbo.CenterDevices", "DeviceUnitId");
            AddForeignKey("dbo.CenterDevices", "DeviceUnitId", "dbo.DeviceUnits", "Id", cascadeDelete: true);
        }
    }
}
