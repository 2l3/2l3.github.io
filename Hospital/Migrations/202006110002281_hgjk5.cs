namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hgjk5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "ImgPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Devices", "ImgPath");
        }
    }
}
