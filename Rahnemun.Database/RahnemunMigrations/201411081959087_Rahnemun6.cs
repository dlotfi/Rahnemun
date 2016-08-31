namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_Categories", "Description", c => c.String(nullable: false, maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rahnemun_Categories", "Description");
        }
    }
}
