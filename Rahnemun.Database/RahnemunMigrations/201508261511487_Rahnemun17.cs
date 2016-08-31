namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_Payments", "RequestResult", c => c.String(maxLength: 5));
            AddColumn("dbo.Rahnemun_Payments", "VerificationResult", c => c.String(maxLength: 5));
            DropColumn("dbo.Rahnemun_Payments", "Result");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rahnemun_Payments", "Result", c => c.String(maxLength: 5));
            DropColumn("dbo.Rahnemun_Payments", "VerificationResult");
            DropColumn("dbo.Rahnemun_Payments", "RequestResult");
        }
    }
}
