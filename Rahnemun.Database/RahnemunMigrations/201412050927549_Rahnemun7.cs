namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_Consultants", "Approved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Rahnemun_Users", "RegisterDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rahnemun_Users", "RegisterDate");
            DropColumn("dbo.Rahnemun_Consultants", "Approved");
        }
    }
}
