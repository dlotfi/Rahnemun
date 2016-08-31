namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun23 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rahnemun_Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 2000),
                        SentTime = c.DateTime(nullable: false),
                        BlogPostId = c.Int(nullable: false),
                        UserId = c.Int(),
                        GuestId = c.Int(),
                        RepliedCommentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rahnemun_BlogPosts", t => t.BlogPostId, cascadeDelete: true)
                .ForeignKey("dbo.Rahnemun_Guests", t => t.GuestId)
                .ForeignKey("dbo.Rahnemun_Comments", t => t.RepliedCommentId)
                .ForeignKey("dbo.Rahnemun_Users", t => t.UserId)
                .Index(t => t.BlogPostId)
                .Index(t => t.UserId)
                .Index(t => t.GuestId)
                .Index(t => t.RepliedCommentId);
            
            CreateTable(
                "dbo.Rahnemun_Guests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 255),
                        Name = c.String(maxLength: 30),
                        UserAgent = c.String(nullable: false, maxLength: 512),
                        UserIP = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Rahnemun_CustomerMessages", "GuestId", c => c.Int());
            CreateIndex("dbo.Rahnemun_CustomerMessages", "UserId");
            CreateIndex("dbo.Rahnemun_CustomerMessages", "GuestId");
            AddForeignKey("dbo.Rahnemun_CustomerMessages", "GuestId", "dbo.Rahnemun_Guests", "Id");
            AddForeignKey("dbo.Rahnemun_CustomerMessages", "UserId", "dbo.Rahnemun_Users", "Id");
            DropColumn("dbo.Rahnemun_CustomerMessages", "FullName");
            DropColumn("dbo.Rahnemun_CustomerMessages", "Email");
            DropColumn("dbo.Rahnemun_CustomerMessages", "UserAgent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rahnemun_CustomerMessages", "UserAgent", c => c.String(nullable: false, maxLength: 512));
            AddColumn("dbo.Rahnemun_CustomerMessages", "Email", c => c.String(maxLength: 255));
            AddColumn("dbo.Rahnemun_CustomerMessages", "FullName", c => c.String(maxLength: 30));
            DropForeignKey("dbo.Rahnemun_CustomerMessages", "UserId", "dbo.Rahnemun_Users");
            DropForeignKey("dbo.Rahnemun_CustomerMessages", "GuestId", "dbo.Rahnemun_Guests");
            DropForeignKey("dbo.Rahnemun_Comments", "UserId", "dbo.Rahnemun_Users");
            DropForeignKey("dbo.Rahnemun_Comments", "RepliedCommentId", "dbo.Rahnemun_Comments");
            DropForeignKey("dbo.Rahnemun_Comments", "GuestId", "dbo.Rahnemun_Guests");
            DropForeignKey("dbo.Rahnemun_Comments", "BlogPostId", "dbo.Rahnemun_BlogPosts");
            DropIndex("dbo.Rahnemun_CustomerMessages", new[] { "GuestId" });
            DropIndex("dbo.Rahnemun_CustomerMessages", new[] { "UserId" });
            DropIndex("dbo.Rahnemun_Comments", new[] { "RepliedCommentId" });
            DropIndex("dbo.Rahnemun_Comments", new[] { "GuestId" });
            DropIndex("dbo.Rahnemun_Comments", new[] { "UserId" });
            DropIndex("dbo.Rahnemun_Comments", new[] { "BlogPostId" });
            DropColumn("dbo.Rahnemun_CustomerMessages", "GuestId");
            DropTable("dbo.Rahnemun_Guests");
            DropTable("dbo.Rahnemun_Comments");
        }
    }
}
