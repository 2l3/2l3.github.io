namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdkjvn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CenterDevices", "DamanExpireDate", c => c.DateTime());
            AlterColumn("dbo.Devices", "DamanExpireDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Devices", "DamanExpireDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CenterDevices", "DamanExpireDate", c => c.DateTime(nullable: false));
        }
    }
}
