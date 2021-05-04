namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ghjk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "Daman", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Devices", "Daman");
        }
    }
}
