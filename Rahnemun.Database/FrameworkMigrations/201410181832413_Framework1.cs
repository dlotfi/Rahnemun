namespace Rahnemun.Database.FrameworkMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Framework1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Framework_Modules",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Framework_Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 255),
                        Email = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false),
                        PasswordSalt = c.String(nullable: false),
                        HashAlgorithm = c.String(nullable: false, maxLength: 255),
                        PasswordFormat = c.Int(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Framework_Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        DisplayName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Framework_RolesPermissions",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        PermissionName = c.String(nullable: false, maxLength: 128),
                        ModuleName = c.String(),
                    })
                .PrimaryKey(t => new { t.RoleId, t.PermissionName })
                .ForeignKey("dbo.Framework_Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Framework_Media",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Path = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Framework_Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.String(nullable: false, maxLength: 255),
                        Name = c.String(nullable: false, maxLength: 255),
                        Value = c.String(),
                        ModuleName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Framework_UsersRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Framework_Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Framework_Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Framework_RolesPermissions", "RoleId", "dbo.Framework_Roles");
            DropForeignKey("dbo.Framework_UsersRoles", "RoleId", "dbo.Framework_Roles");
            DropForeignKey("dbo.Framework_UsersRoles", "UserId", "dbo.Framework_Users");
            DropIndex("dbo.Framework_UsersRoles", new[] { "RoleId" });
            DropIndex("dbo.Framework_UsersRoles", new[] { "UserId" });
            DropIndex("dbo.Framework_RolesPermissions", new[] { "RoleId" });
            DropTable("dbo.Framework_UsersRoles");
            DropTable("dbo.Framework_Settings");
            DropTable("dbo.Framework_Media");
            DropTable("dbo.Framework_RolesPermissions");
            DropTable("dbo.Framework_Roles");
            DropTable("dbo.Framework_Users");
            DropTable("dbo.Framework_Modules");
        }
    }
}
