namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fvghjk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceRequests", "ToUnitAr", c => c.String());
            AddColumn("dbo.DeviceRequests", "ToUnitEn", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceRequests", "ToUnitEn");
            DropColumn("dbo.DeviceRequests", "ToUnitAr");
        }
    }
}
