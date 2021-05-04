namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fffff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CenterDeviceRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reason = c.String(),
                        DeviceId = c.Int(),
                        RequestType = c.Int(nullable: false),
                        DeviceUnitId = c.Int(),
                        ConfirmedDate = c.DateTime(),
                        ConfirmType = c.Int(nullable: false),
                        User = c.String(),
                        ToUnitAr = c.String(),
                        ToUnitEn = c.String(),
                        DeleteReason = c.String(),
                        DeviceName = c.String(),
                        ComputerNumber = c.String(),
                        WithNoBarCode = c.Boolean(),
                        IsActive = c.Boolean(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(),
                        DeletedBy = c.Int(),
                        DeletedDate = c.DateTime(),
                        CreatorName = c.String(),
                        DeleterName = c.String(),
                        ModifierName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CenterDevices", t => t.DeviceId)
                .Index(t => t.DeviceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CenterDeviceRequests", "DeviceId", "dbo.CenterDevices");
            DropIndex("dbo.CenterDeviceRequests", new[] { "DeviceId" });
            DropTable("dbo.CenterDeviceRequests");
        }
    }
}
