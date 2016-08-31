namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rahnemun_Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        StopTime = c.DateTime(),
                        StopType = c.Byte(),
                        Rating = c.Byte(),
                        StarterId = c.Int(nullable: false),
                        InviteeId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rahnemun_Users", t => t.InviteeId)
                .ForeignKey("dbo.Rahnemun_Users", t => t.StarterId)
                .ForeignKey("dbo.Rahnemun_Categories", t => t.CategoryId)
                .Index(t => t.StarterId)
                .Index(t => t.InviteeId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Rahnemun_Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        ByStarter = c.Boolean(nullable: false),
                        SentTime = c.DateTime(nullable: false),
                        SeenTime = c.DateTime(),
                        MediaId = c.Int(),
                        SessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rahnemun_Sessions", t => t.SessionId, cascadeDelete: true)
                .Index(t => t.SessionId);
            
            CreateTable(
                "dbo.Rahnemun_StopRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestTime = c.DateTime(nullable: false),
                        RejectTime = c.DateTime(),
                        SessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rahnemun_Sessions", t => t.SessionId, cascadeDelete: true)
                .Index(t => t.SessionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rahnemun_Sessions", "CategoryId", "dbo.Rahnemun_Categories");
            DropForeignKey("dbo.Rahnemun_StopRequests", "SessionId", "dbo.Rahnemun_Sessions");
            DropForeignKey("dbo.Rahnemun_Messages", "SessionId", "dbo.Rahnemun_Sessions");
            DropForeignKey("dbo.Rahnemun_Sessions", "StarterId", "dbo.Rahnemun_Users");
            DropForeignKey("dbo.Rahnemun_Sessions", "InviteeId", "dbo.Rahnemun_Users");
            DropIndex("dbo.Rahnemun_StopRequests", new[] { "SessionId" });
            DropIndex("dbo.Rahnemun_Messages", new[] { "SessionId" });
            DropIndex("dbo.Rahnemun_Sessions", new[] { "CategoryId" });
            DropIndex("dbo.Rahnemun_Sessions", new[] { "InviteeId" });
            DropIndex("dbo.Rahnemun_Sessions", new[] { "StarterId" });
            DropTable("dbo.Rahnemun_StopRequests");
            DropTable("dbo.Rahnemun_Messages");
            DropTable("dbo.Rahnemun_Sessions");
        }
    }
}
