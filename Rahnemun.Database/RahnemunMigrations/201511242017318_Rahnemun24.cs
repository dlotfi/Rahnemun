namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_BlogPosts", "UrlTitle", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rahnemun_BlogPosts", "UrlTitle");
        }
    }
}
