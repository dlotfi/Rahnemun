namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun26 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_BlogPosts", "CallToAction", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rahnemun_BlogPosts", "CallToAction");
        }
    }
}
