namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uyasgjh1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "TitleEn", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "TitleEn");
        }
    }
}
