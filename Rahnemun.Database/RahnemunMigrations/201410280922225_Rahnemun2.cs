namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_Consultants", "BankCardNo", c => c.String(nullable: false, maxLength: 16));
            AddColumn("dbo.Rahnemun_Consultants", "BankAccountNo", c => c.String(maxLength: 30));
            AddColumn("dbo.Rahnemun_Consultants", "BankName", c => c.String(maxLength: 50));
            AddColumn("dbo.Rahnemun_Consultants", "Education", c => c.String(nullable: false, maxLength: 1000));
            AddColumn("dbo.Rahnemun_Consultants", "ProfessionalExperience", c => c.String(nullable: false, maxLength: 1000));
            AddColumn("dbo.Rahnemun_Consultants", "LicenseNumber", c => c.String(maxLength: 30));
            AddColumn("dbo.Rahnemun_Consultants", "ProfessionalCertificates", c => c.String(maxLength: 1000));
            AddColumn("dbo.Rahnemun_Consultants", "WorkAddress", c => c.String(nullable: false, maxLength: 300));
            AddColumn("dbo.Rahnemun_Consultants", "WorkPhoneNo", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Rahnemun_Consultants", "Capacity", c => c.Byte(nullable: false));
            AddColumn("dbo.Rahnemun_Users", "EducationLevel", c => c.Byte(nullable: false));
            AddColumn("dbo.Rahnemun_Users", "NationalId", c => c.String(maxLength: 10));
            AddColumn("dbo.Rahnemun_Users", "MaritalStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.Rahnemun_Users", "CellphoneNo", c => c.String(maxLength: 11));
            AddColumn("dbo.Rahnemun_Users", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Rahnemun_Users", "More", c => c.String(maxLength: 2000));
            AlterColumn("dbo.Rahnemun_Consultants", "Specialty", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Rahnemun_Users", "FirstName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Rahnemun_Users", "LastName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Rahnemun_Users", "Gender", c => c.Byte(nullable: false));
            DropColumn("dbo.Rahnemun_Users", "Education");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rahnemun_Users", "Education", c => c.Int(nullable: false));
            AlterColumn("dbo.Rahnemun_Users", "Gender", c => c.Int(nullable: false));
            AlterColumn("dbo.Rahnemun_Users", "LastName", c => c.String());
            AlterColumn("dbo.Rahnemun_Users", "FirstName", c => c.String());
            AlterColumn("dbo.Rahnemun_Consultants", "Specialty", c => c.String());
            DropColumn("dbo.Rahnemun_Users", "More");
            DropColumn("dbo.Rahnemun_Users", "BirthDate");
            DropColumn("dbo.Rahnemun_Users", "CellphoneNo");
            DropColumn("dbo.Rahnemun_Users", "MaritalStatus");
            DropColumn("dbo.Rahnemun_Users", "NationalId");
            DropColumn("dbo.Rahnemun_Users", "EducationLevel");
            DropColumn("dbo.Rahnemun_Consultants", "Capacity");
            DropColumn("dbo.Rahnemun_Consultants", "WorkPhoneNo");
            DropColumn("dbo.Rahnemun_Consultants", "WorkAddress");
            DropColumn("dbo.Rahnemun_Consultants", "ProfessionalCertificates");
            DropColumn("dbo.Rahnemun_Consultants", "LicenseNumber");
            DropColumn("dbo.Rahnemun_Consultants", "ProfessionalExperience");
            DropColumn("dbo.Rahnemun_Consultants", "Education");
            DropColumn("dbo.Rahnemun_Consultants", "BankName");
            DropColumn("dbo.Rahnemun_Consultants", "BankAccountNo");
            DropColumn("dbo.Rahnemun_Consultants", "BankCardNo");
        }
    }
}
