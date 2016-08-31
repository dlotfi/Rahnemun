namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_Consultants", "Fee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Rahnemun_Categories", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rahnemun_Categories", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Rahnemun_Consultants", "Fee");
        }
    }
}
