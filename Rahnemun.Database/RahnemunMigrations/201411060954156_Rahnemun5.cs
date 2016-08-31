namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rahnemun_Categories", "CategoryGroup_Id", "dbo.Rahnemun_CategoryGroups");
            DropIndex("dbo.Rahnemun_Categories", new[] { "CategoryGroup_Id" });
            RenameColumn(table: "dbo.Rahnemun_Categories", name: "CategoryGroup_Id", newName: "CategoryGroupId");
            AddColumn("dbo.Rahnemun_Categories", "DisplayOrder", c => c.Byte(nullable: false));
            AddColumn("dbo.Rahnemun_CategoryGroups", "DisplayOrder", c => c.Byte(nullable: false));
            AlterColumn("dbo.Rahnemun_Categories", "CategoryGroupId", c => c.Int(nullable: false));
            CreateIndex("dbo.Rahnemun_Categories", "CategoryGroupId");
            AddForeignKey("dbo.Rahnemun_Categories", "CategoryGroupId", "dbo.Rahnemun_CategoryGroups", "Id", cascadeDelete: true);
            Sql("Update dbo.Rahnemun_Categories " +
                "SET dbo.Rahnemun_Categories.DisplayOrder = Temp.DisplayOrder " +
                "FROM dbo.Rahnemun_Categories  " +
                "INNER JOIN (SELECT Id, ROW_NUMBER() OVER(PARTITION BY [CategoryGroupId] ORDER BY [CategoryGroupId]) AS [DisplayOrder] FROM dbo.Rahnemun_Categories) AS Temp " +
                "ON Temp.Id = dbo.Rahnemun_Categories.Id");
            Sql("Update dbo.Rahnemun_CategoryGroups " +
                "SET dbo.Rahnemun_CategoryGroups.DisplayOrder = Temp.DisplayOrder " +
                "FROM dbo.Rahnemun_CategoryGroups " +
                "INNER JOIN (SELECT [Id], ROW_NUMBER() OVER(ORDER BY [Id]) AS [DisplayOrder] FROM [Rahnemun].[dbo].[Rahnemun_CategoryGroups]) AS Temp " +
                "ON Temp.Id = dbo.Rahnemun_CategoryGroups.Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rahnemun_Categories", "CategoryGroupId", "dbo.Rahnemun_CategoryGroups");
            DropIndex("dbo.Rahnemun_Categories", new[] { "CategoryGroupId" });
            AlterColumn("dbo.Rahnemun_Categories", "CategoryGroupId", c => c.Int());
            DropColumn("dbo.Rahnemun_CategoryGroups", "DisplayOrder");
            DropColumn("dbo.Rahnemun_Categories", "DisplayOrder");
            RenameColumn(table: "dbo.Rahnemun_Categories", name: "CategoryGroupId", newName: "CategoryGroup_Id");
            CreateIndex("dbo.Rahnemun_Categories", "CategoryGroup_Id");
            AddForeignKey("dbo.Rahnemun_Categories", "CategoryGroup_Id", "dbo.Rahnemun_CategoryGroups", "Id");
        }
    }
}
