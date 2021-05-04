namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sldjbvn55 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CenterDeviceSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CenterDeviceId = c.Int(nullable: false),
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
                .ForeignKey("dbo.CenterDevices", t => t.CenterDeviceId, cascadeDelete: true)
                .ForeignKey("dbo.Days", t => t.DayId, cascadeDelete: true)
                .ForeignKey("dbo.Months", t => t.MonthId, cascadeDelete: true)
                .Index(t => t.CenterDeviceId)
                .Index(t => t.DayId)
                .Index(t => t.MonthId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CenterDeviceSchedules", "MonthId", "dbo.Months");
            DropForeignKey("dbo.CenterDeviceSchedules", "DayId", "dbo.Days");
            DropForeignKey("dbo.CenterDeviceSchedules", "CenterDeviceId", "dbo.CenterDevices");
            DropIndex("dbo.CenterDeviceSchedules", new[] { "MonthId" });
            DropIndex("dbo.CenterDeviceSchedules", new[] { "DayId" });
            DropIndex("dbo.CenterDeviceSchedules", new[] { "CenterDeviceId" });
            DropTable("dbo.CenterDeviceSchedules");
        }
    }
}
