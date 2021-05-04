namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sldjbvn555 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CenterReportSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DescAr = c.String(),
                        DescEn = c.String(),
                        Sort = c.Int(nullable: false),
                        FontSize = c.Int(nullable: false),
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
                "dbo.CenterReportSetting2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DescAr = c.String(),
                        DescEn = c.String(),
                        Sort = c.Int(nullable: false),
                        FontSize = c.Int(nullable: false),
                        Above = c.Int(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CenterReportSetting2");
            DropTable("dbo.CenterReportSettings");
        }
    }
}
