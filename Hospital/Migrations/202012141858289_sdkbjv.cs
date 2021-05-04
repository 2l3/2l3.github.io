namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdkbjv : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Centers", "Password", c => c.String());
            AddColumn("dbo.WebsiteDatas", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WebsiteDatas", "Password");
            DropColumn("dbo.Centers", "Password");
        }
    }
}
