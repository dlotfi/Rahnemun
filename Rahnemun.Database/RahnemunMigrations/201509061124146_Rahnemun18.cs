namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_BlogPosts", "ThumbnailPictureId", c => c.Int(nullable: false));
            AlterColumn("dbo.Rahnemun_BlogPosts", "CoverPictureId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rahnemun_BlogPosts", "CoverPictureId", c => c.Int());
            DropColumn("dbo.Rahnemun_BlogPosts", "ThumbnailPictureId");
        }
    }
}
