namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun22 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rahnemun_StopRequests", "SessionId", "dbo.Rahnemun_Sessions");
            DropIndex("dbo.Rahnemun_StopRequests", new[] { "SessionId" });
            RenameColumn(table: "dbo.Rahnemun_Sessions", name: "InviteeId", newName: "ConsultantId");
            RenameColumn(table: "dbo.Rahnemun_Sessions", name: "StarterId", newName: "ConsulteeId");
            RenameIndex(table: "dbo.Rahnemun_Sessions", name: "IX_StarterId", newName: "IX_ConsulteeId");
            RenameIndex(table: "dbo.Rahnemun_Sessions", name: "IX_InviteeId", newName: "IX_ConsultantId");
            //AddColumn("dbo.Rahnemun_Messages", "ByConsultee", c => c.Boolean(nullable: false));
            //DropColumn("dbo.Rahnemun_Messages", "ByStarter");
            RenameColumn(table: "dbo.Rahnemun_Messages", name: "ByStarter", newName: "ByConsultee");
            DropTable("dbo.Rahnemun_StopRequests");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Rahnemun_StopRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestTime = c.DateTime(nullable: false),
                        RejectTime = c.DateTime(),
                        SessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            RenameColumn(table: "dbo.Rahnemun_Messages", name: "ByConsultee", newName: "ByStarter");
            //AddColumn("dbo.Rahnemun_Messages", "ByStarter", c => c.Boolean(nullable: false));
            //DropColumn("dbo.Rahnemun_Messages", "ByConsultee");
            RenameIndex(table: "dbo.Rahnemun_Sessions", name: "IX_ConsultantId", newName: "IX_InviteeId");
            RenameIndex(table: "dbo.Rahnemun_Sessions", name: "IX_ConsulteeId", newName: "IX_StarterId");
            RenameColumn(table: "dbo.Rahnemun_Sessions", name: "ConsulteeId", newName: "StarterId");
            RenameColumn(table: "dbo.Rahnemun_Sessions", name: "ConsultantId", newName: "InviteeId");
            CreateIndex("dbo.Rahnemun_StopRequests", "SessionId");
            AddForeignKey("dbo.Rahnemun_StopRequests", "SessionId", "dbo.Rahnemun_Sessions", "Id", cascadeDelete: true);
        }
    }
}
