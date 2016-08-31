namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rahnemun_CategoryGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Caption = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Rahnemun_Categories", "Caption", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Rahnemun_Categories", "CategoryGroup_Id", c => c.Int());
            CreateIndex("dbo.Rahnemun_Categories", "CategoryGroup_Id");
            AddForeignKey("dbo.Rahnemun_Categories", "CategoryGroup_Id", "dbo.Rahnemun_CategoryGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rahnemun_Categories", "CategoryGroup_Id", "dbo.Rahnemun_CategoryGroups");
            DropIndex("dbo.Rahnemun_Categories", new[] { "CategoryGroup_Id" });
            DropColumn("dbo.Rahnemun_Categories", "CategoryGroup_Id");
            DropColumn("dbo.Rahnemun_Categories", "Caption");
            DropTable("dbo.Rahnemun_CategoryGroups");
        }
    }
}
