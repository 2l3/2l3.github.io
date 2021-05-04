namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fghjk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceRequests", "User", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceRequests", "User");
        }
    }
}
