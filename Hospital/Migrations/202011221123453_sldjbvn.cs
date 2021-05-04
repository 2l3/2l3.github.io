namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sldjbvn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeviceSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceId = c.Int(nullable: false),
                        DayId = c.Int(nullable: false),
                        MonthId = c.Int(nullable: false),
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
                .ForeignKey("dbo.Days", t => t.DayId, cascadeDelete: true)
                .ForeignKey("dbo.Devices", t => t.DeviceId, cascadeDelete: true)
                .ForeignKey("dbo.Months", t => t.MonthId, cascadeDelete: true)
                .Index(t => t.DeviceId)
                .Index(t => t.DayId)
                .Index(t => t.MonthId);
            
            CreateTable(
                "dbo.Months",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameAr = c.String(),
                        NameEn = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceSchedules", "MonthId", "dbo.Months");
            DropForeignKey("dbo.DeviceSchedules", "DeviceId", "dbo.Devices");
            DropForeignKey("dbo.DeviceSchedules", "DayId", "dbo.Days");
            DropIndex("dbo.DeviceSchedules", new[] { "MonthId" });
            DropIndex("dbo.DeviceSchedules", new[] { "DayId" });
            DropIndex("dbo.DeviceSchedules", new[] { "DeviceId" });
            DropTable("dbo.Months");
            DropTable("dbo.DeviceSchedules");
            DropTable("dbo.Days");
        }
    }
}
