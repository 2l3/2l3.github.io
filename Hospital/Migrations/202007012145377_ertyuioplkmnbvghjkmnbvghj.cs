namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ertyuioplkmnbvghjkmnbvghj : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportSettings",
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
                "dbo.WebsiteDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImgPath = c.String(),
                        PPMStickerText = c.String(),
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
            DropTable("dbo.WebsiteDatas");
            DropTable("dbo.ReportSettings");
        }
    }
}
