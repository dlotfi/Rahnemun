namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun27 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Rahnemun_BlogPosts", "UrlTitle", "Slug");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Rahnemun_BlogPosts", "Slug", "UrlTitle");
        }
    }
}
