namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdjbvn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DCenterDevices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameEn = c.String(),
                        NameAr = c.String(),
                        Com = c.String(),
                        Serial = c.String(),
                        Model = c.String(),
                        ComputerNumber = c.String(),
                        Desc = c.String(),
                        ImgPath = c.String(),
                        ShowDaman = c.Boolean(nullable: false),
                        DamanExpireDate = c.DateTime(),
                        CompanyName = c.String(),
                        DeviceTypeId = c.Int(),
                        CenterDeviceUnitId = c.Int(nullable: false),
                        DeviceCategoryId = c.Int(nullable: false),
                        ShowLastModification = c.Boolean(nullable: false),
                        LastModificationDate = c.DateTime(),
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
                .ForeignKey("dbo.CenterDeviceUnits", t => t.CenterDeviceUnitId, cascadeDelete: true)
                .ForeignKey("dbo.DeviceTypes", t => t.DeviceTypeId)
                .Index(t => t.DeviceTypeId)
                .Index(t => t.CenterDeviceUnitId);
            
            CreateTable(
                "dbo.DDevices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameEn = c.String(),
                        NameAr = c.String(),
                        Com = c.String(),
                        Serial = c.String(),
                        Model = c.String(),
                        ComputerNumber = c.String(),
                        DeviceUnitId = c.Int(nullable: false),
                        Desc = c.String(),
                        ImgPath = c.String(),
                        DeviceTypeId = c.Int(),
                        DeviceCategoryId = c.Int(nullable: false),
                        ShowLastModification = c.Boolean(nullable: false),
                        ShowDaman = c.Boolean(nullable: false),
                        DamanExpireDate = c.DateTime(),
                        LastModificationDate = c.DateTime(),
                        CompanyName = c.String(),
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
                .ForeignKey("dbo.DeviceTypes", t => t.DeviceTypeId)
                .ForeignKey("dbo.DeviceUnits", t => t.DeviceUnitId, cascadeDelete: true)
                .Index(t => t.DeviceUnitId)
                .Index(t => t.DeviceTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DDevices", "DeviceUnitId", "dbo.DeviceUnits");
            DropForeignKey("dbo.DDevices", "DeviceTypeId", "dbo.DeviceTypes");
            DropForeignKey("dbo.DCenterDevices", "DeviceTypeId", "dbo.DeviceTypes");
            DropForeignKey("dbo.DCenterDevices", "CenterDeviceUnitId", "dbo.CenterDeviceUnits");
            DropIndex("dbo.DDevices", new[] { "DeviceTypeId" });
            DropIndex("dbo.DDevices", new[] { "DeviceUnitId" });
            DropIndex("dbo.DCenterDevices", new[] { "CenterDeviceUnitId" });
            DropIndex("dbo.DCenterDevices", new[] { "DeviceTypeId" });
            DropTable("dbo.DDevices");
            DropTable("dbo.DCenterDevices");
        }
    }
}
