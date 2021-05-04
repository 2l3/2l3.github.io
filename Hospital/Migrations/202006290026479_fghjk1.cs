namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fghjk1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Engineers",
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Engineers");
        }
    }
}
