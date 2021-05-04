namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sjdkbnv655888 : DbMigration
    {
        public override void Up()
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
            
            CreateIndex("dbo.CenterDevices", "DeviceTypeId");
            CreateIndex("dbo.Devices", "DeviceTypeId");
            AddForeignKey("dbo.CenterDevices", "DeviceTypeId", "dbo.DeviceTypes", "Id");
            AddForeignKey("dbo.Devices", "DeviceTypeId", "dbo.DeviceTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devices", "DeviceTypeId", "dbo.DeviceTypes");
            DropForeignKey("dbo.CenterDevices", "DeviceTypeId", "dbo.DeviceTypes");
            DropIndex("dbo.Devices", new[] { "DeviceTypeId" });
            DropIndex("dbo.CenterDevices", new[] { "DeviceTypeId" });
            DropTable("dbo.DeviceTypes");
        }
    }
}
