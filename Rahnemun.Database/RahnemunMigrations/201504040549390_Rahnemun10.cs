namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rahnemun_Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HandlerId = c.String(nullable: false, maxLength: 50),
                        HandlerData = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rahnemun_Users", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Rahnemun_Categories", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rahnemun_Categories", "Terms", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rahnemun_Payments", "UserId", "dbo.Rahnemun_Users");
            DropIndex("dbo.Rahnemun_Payments", new[] { "UserId" });
            DropColumn("dbo.Rahnemun_Categories", "Terms");
            DropColumn("dbo.Rahnemun_Categories", "Price");
            DropTable("dbo.Rahnemun_Payments");
        }
    }
}
