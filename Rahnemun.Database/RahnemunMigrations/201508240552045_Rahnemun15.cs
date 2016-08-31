namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_Sessions", "PaymentId", c => c.Int(nullable: false));
            AddColumn("dbo.Rahnemun_Payments", "ReferenceId", c => c.String(maxLength: 50));
            CreateIndex("dbo.Rahnemun_Sessions", "PaymentId");
            AddForeignKey("dbo.Rahnemun_Sessions", "PaymentId", "dbo.Rahnemun_Payments", "Id");
            DropColumn("dbo.Rahnemun_Payments", "RefrencedId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rahnemun_Payments", "RefrencedId", c => c.String(maxLength: 50));
            DropForeignKey("dbo.Rahnemun_Sessions", "PaymentId", "dbo.Rahnemun_Payments");
            DropIndex("dbo.Rahnemun_Sessions", new[] { "PaymentId" });
            DropColumn("dbo.Rahnemun_Payments", "ReferenceId");
            DropColumn("dbo.Rahnemun_Sessions", "PaymentId");
        }
    }
}
