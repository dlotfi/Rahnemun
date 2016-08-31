namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun21 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Rahnemun_Consultants", "Specialty", "Title");
            AlterColumn("dbo.Rahnemun_Consultants", "WorkAddress", c => c.String(maxLength: 300));
            AlterColumn("dbo.Rahnemun_Consultants", "WorkPhoneNo", c => c.String(maxLength: 50));
            DropColumn("dbo.Rahnemun_Users", "NationalId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rahnemun_Users", "NationalId", c => c.String(maxLength: 10));
            AlterColumn("dbo.Rahnemun_Consultants", "WorkPhoneNo", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Rahnemun_Consultants", "WorkAddress", c => c.String(nullable: false, maxLength: 300));
            RenameColumn("dbo.Rahnemun_Consultants", "Title", "Specialty");
        }
    }
}
