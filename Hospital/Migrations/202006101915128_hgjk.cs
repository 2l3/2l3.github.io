namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hgjk : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameEn = c.String(),
                        NameAr = c.String(),
                        DeviceUnitId = c.Int(nullable: false),
                        Desc = c.String(),
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
                .ForeignKey("dbo.DeviceUnits", t => t.DeviceUnitId, cascadeDelete: true)
                .Index(t => t.DeviceUnitId);
            
            CreateTable(
                "dbo.DeviceUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameAr = c.String(),
                        NameEn = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeviceRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reason = c.String(),
                        DeviceId = c.Int(nullable: false),
                        RequestType = c.Int(nullable: false),
                        DeviceUnitId = c.Int(),
                        ConfirmedDate = c.DateTime(),
                        IsConfirmed = c.Boolean(nullable: false),
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
                .ForeignKey("dbo.Devices", t => t.DeviceId, cascadeDelete: true)
                .Index(t => t.DeviceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceRequests", "DeviceId", "dbo.Devices");
            DropForeignKey("dbo.Devices", "DeviceUnitId", "dbo.DeviceUnits");
            DropIndex("dbo.DeviceRequests", new[] { "DeviceId" });
            DropIndex("dbo.Devices", new[] { "DeviceUnitId" });
            DropTable("dbo.DeviceRequests");
            DropTable("dbo.DeviceUnits");
            DropTable("dbo.Devices");
        }
    }
}
