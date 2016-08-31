namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun20 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rahnemun_Consultants", "CategoryId", "dbo.Rahnemun_Categories");
            DropIndex("dbo.Rahnemun_Consultants", new[] { "CategoryId" });
            CreateTable(
                "dbo.Rahnemun_ConsultantsCategories",
                c => new
                    {
                        ConsultantId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ConsultantId, t.CategoryId })
                .ForeignKey("dbo.Rahnemun_Consultants", t => t.ConsultantId, cascadeDelete: true)
                .ForeignKey("dbo.Rahnemun_Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.ConsultantId)
                .Index(t => t.CategoryId);
            
            DropColumn("dbo.Rahnemun_Consultants", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rahnemun_Consultants", "CategoryId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Rahnemun_ConsultantsCategories", "CategoryId", "dbo.Rahnemun_Categories");
            DropForeignKey("dbo.Rahnemun_ConsultantsCategories", "ConsultantId", "dbo.Rahnemun_Consultants");
            DropIndex("dbo.Rahnemun_ConsultantsCategories", new[] { "CategoryId" });
            DropIndex("dbo.Rahnemun_ConsultantsCategories", new[] { "ConsultantId" });
            DropTable("dbo.Rahnemun_ConsultantsCategories");
            CreateIndex("dbo.Rahnemun_Consultants", "CategoryId");
            AddForeignKey("dbo.Rahnemun_Consultants", "CategoryId", "dbo.Rahnemun_Categories", "Id");
        }
    }
}
