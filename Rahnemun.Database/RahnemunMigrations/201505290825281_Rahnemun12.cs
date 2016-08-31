namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_Payments", "RequestTime", c => c.DateTime());
            AddColumn("dbo.Rahnemun_Payments", "VerificationTime", c => c.DateTime());
            AlterColumn("dbo.Rahnemun_Payments", "Time", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Rahnemun_Users", "Gender", c => c.Byte());
            AlterColumn("dbo.Rahnemun_Users", "EducationLevel", c => c.Byte());
            AlterColumn("dbo.Rahnemun_Users", "MaritalStatus", c => c.Byte());
            AlterColumn("dbo.Rahnemun_Users", "BirthDate", c => c.DateTime());    
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rahnemun_Users", "BirthDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Rahnemun_Users", "MaritalStatus", c => c.Byte(nullable: false));
            AlterColumn("dbo.Rahnemun_Users", "EducationLevel", c => c.Byte(nullable: false));
            AlterColumn("dbo.Rahnemun_Users", "Gender", c => c.Byte(nullable: false));
            AlterColumn("dbo.Rahnemun_Payments", "Time", c => c.DateTime());
            DropColumn("dbo.Rahnemun_Payments", "VerificationTime");
            DropColumn("dbo.Rahnemun_Payments", "RequestTime");
        }
    }
}
