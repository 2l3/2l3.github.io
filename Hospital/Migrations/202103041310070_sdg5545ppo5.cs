namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdg5545ppo5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CenterDeviceUnits", "CustodyOfficial", c => c.String());
            AddColumn("dbo.CenterDeviceUnits", "CustodyOPhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CenterDeviceUnits", "CustodyOPhone");
            DropColumn("dbo.CenterDeviceUnits", "CustodyOfficial");
        }
    }
}
