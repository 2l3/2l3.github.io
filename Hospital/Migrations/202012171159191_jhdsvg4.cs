namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jhdsvg4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CenterDeviceRequests", "CenterNameAr", c => c.String());
            AddColumn("dbo.CenterDeviceRequests", "CenterNameEn", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CenterDeviceRequests", "CenterNameEn");
            DropColumn("dbo.CenterDeviceRequests", "CenterNameAr");
        }
    }
}
