namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun16 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rahnemun_BlogPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        Summary = c.String(nullable: false, maxLength: 1000),
                        Content = c.String(nullable: false),
                        PublishTime = c.DateTime(nullable: false),
                        Category = c.String(nullable: false, maxLength: 255),
                        Tags = c.String(maxLength: 512),
                        UserId = c.Int(nullable: false),
                        CoverPictureId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rahnemun_Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rahnemun_BlogPosts", "UserId", "dbo.Rahnemun_Users");
            DropIndex("dbo.Rahnemun_BlogPosts", new[] { "UserId" });
            DropTable("dbo.Rahnemun_BlogPosts");
        }
    }
}
