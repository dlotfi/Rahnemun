namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rahnemun_Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rahnemun_Users",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.Int(nullable: false),
                        Education = c.Int(nullable: false),
                        ProfilePictureId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rahnemun_Consultants",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Specialty = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rahnemun_Users", t => t.Id)
                .ForeignKey("dbo.Rahnemun_Categories", t => t.CategoryId)
                .Index(t => t.Id)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rahnemun_Consultants", "CategoryId", "dbo.Rahnemun_Categories");
            DropForeignKey("dbo.Rahnemun_Consultants", "Id", "dbo.Rahnemun_Users");
            DropIndex("dbo.Rahnemun_Consultants", new[] { "CategoryId" });
            DropIndex("dbo.Rahnemun_Consultants", new[] { "Id" });
            DropTable("dbo.Rahnemun_Consultants");
            DropTable("dbo.Rahnemun_Users");
            DropTable("dbo.Rahnemun_Categories");
        }
    }
}
