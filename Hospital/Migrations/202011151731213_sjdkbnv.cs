namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sjdkbnv : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Centers",
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
            
            CreateTable(
                "dbo.CenterDevices",
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
                        ShowDaman = c.Boolean(nullable: false),
                        DamanExpireDate = c.DateTime(nullable: false),
                        CompanyName = c.String(),
                        DeviceTypeId = c.Int(),
                        CenterDeviceUnitId = c.Int(nullable: false),
                        DeviceCategoryId = c.Int(nullable: false),
                        ShowLastModification = c.Boolean(nullable: false),
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
                .ForeignKey("dbo.DeviceUnits", t => t.DeviceUnitId, cascadeDelete: true)
                .Index(t => t.DeviceUnitId)
                .Index(t => t.DeviceTypeId)
                .Index(t => t.CenterDeviceUnitId);
            
            CreateTable(
                "dbo.CenterDeviceUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CenterId = c.Int(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Centers", t => t.CenterId, cascadeDelete: true)
                .Index(t => t.CenterId);
            
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
            
            AddColumn("dbo.Devices", "DeviceTypeId", c => c.Int());
            AddColumn("dbo.Devices", "DeviceCategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.Devices", "ShowLastModification", c => c.Boolean(nullable: false));
            AddColumn("dbo.Devices", "ShowDaman", c => c.Boolean(nullable: false));
            AddColumn("dbo.Devices", "DamanExpireDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Devices", "CompanyName", c => c.String());
            AddColumn("dbo.Engineers", "SignImg", c => c.String());
            CreateIndex("dbo.Devices", "DeviceTypeId");
            AddForeignKey("dbo.Devices", "DeviceTypeId", "dbo.DeviceTypes", "Id");
            DropColumn("dbo.Devices", "Daman");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Devices", "Daman", c => c.String());
            DropForeignKey("dbo.Devices", "DeviceTypeId", "dbo.DeviceTypes");
            DropForeignKey("dbo.CenterDevices", "DeviceUnitId", "dbo.DeviceUnits");
            DropForeignKey("dbo.CenterDevices", "DeviceTypeId", "dbo.DeviceTypes");
            DropForeignKey("dbo.CenterDevices", "CenterDeviceUnitId", "dbo.CenterDeviceUnits");
            DropForeignKey("dbo.CenterDeviceUnits", "CenterId", "dbo.Centers");
            DropIndex("dbo.Devices", new[] { "DeviceTypeId" });
            DropIndex("dbo.CenterDeviceUnits", new[] { "CenterId" });
            DropIndex("dbo.CenterDevices", new[] { "CenterDeviceUnitId" });
            DropIndex("dbo.CenterDevices", new[] { "DeviceTypeId" });
            DropIndex("dbo.CenterDevices", new[] { "DeviceUnitId" });
            DropColumn("dbo.Engineers", "SignImg");
            DropColumn("dbo.Devices", "CompanyName");
            DropColumn("dbo.Devices", "DamanExpireDate");
            DropColumn("dbo.Devices", "ShowDaman");
            DropColumn("dbo.Devices", "ShowLastModification");
            DropColumn("dbo.Devices", "DeviceCategoryId");
            DropColumn("dbo.Devices", "DeviceTypeId");
            DropTable("dbo.DeviceTypes");
            DropTable("dbo.CenterDeviceUnits");
            DropTable("dbo.CenterDevices");
            DropTable("dbo.Centers");
        }
    }
}
