namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_Payments", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rahnemun_Payments", "ProviderName", c => c.String(maxLength: 50));
            AddColumn("dbo.Rahnemun_Payments", "RequestId", c => c.String(maxLength: 50));
            AddColumn("dbo.Rahnemun_Payments", "RefrencedId", c => c.String(maxLength: 50));
            AddColumn("dbo.Rahnemun_Payments", "Result", c => c.String(maxLength: 5, fixedLength: true));
            AddColumn("dbo.Rahnemun_Payments", "Time", c => c.DateTime());
            DropColumn("dbo.Rahnemun_Payments", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rahnemun_Payments", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Rahnemun_Payments", "Time");
            DropColumn("dbo.Rahnemun_Payments", "Result");
            DropColumn("dbo.Rahnemun_Payments", "RefrencedId");
            DropColumn("dbo.Rahnemun_Payments", "RequestId");
            DropColumn("dbo.Rahnemun_Payments", "ProviderName");
            DropColumn("dbo.Rahnemun_Payments", "Amount");
        }
    }
}
