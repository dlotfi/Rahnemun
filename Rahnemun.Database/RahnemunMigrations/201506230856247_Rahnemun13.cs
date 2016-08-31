namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_Users", "SubscribedToNewsletter", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rahnemun_Users", "SubscribedToNewsletter");
        }
    }
}
