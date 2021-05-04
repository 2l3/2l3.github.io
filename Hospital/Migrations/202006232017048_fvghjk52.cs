namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fvghjk52 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceRequests", "DeleteReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceRequests", "DeleteReason");
        }
    }
}
