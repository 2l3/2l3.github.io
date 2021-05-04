namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sjdkbnv655 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CenterDevices", "DeviceTypeId", "dbo.DeviceTypes");
            DropForeignKey("dbo.Devices", "DeviceTypeId", "dbo.DeviceTypes");
            DropIndex("dbo.CenterDevices", new[] { "DeviceTypeId" });
            DropIndex("dbo.Devices", new[] { "DeviceTypeId" });
            DropTable("dbo.DeviceTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DeviceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameAr = c.String(),
                        NameEn = c.String(),
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Devices", "DeviceTypeId");
            CreateIndex("dbo.CenterDevices", "DeviceTypeId");
            AddForeignKey("dbo.Devices", "DeviceTypeId", "dbo.DeviceTypes", "Id");
            AddForeignKey("dbo.CenterDevices", "DeviceTypeId", "dbo.DeviceTypes", "Id");
        }
    }
}
