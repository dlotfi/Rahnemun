namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Rahnemun_Consultants", "BankCardNo");
            DropColumn("dbo.Rahnemun_Users", "NationalId");
            DropColumn("dbo.Rahnemun_Users", "CellphoneNo");
            AddColumn("dbo.Rahnemun_Consultants", "BankCardNo", c => c.String(nullable: false, fixedLength: true, maxLength: 16));
            //AddColumn("dbo.Rahnemun_Users", "NationalId", c => c.String(nullable: false, fixedLength: true, maxLength: 10));
            AddColumn("dbo.Rahnemun_Users", "NationalId", c => c.String(nullable: true, fixedLength: true, maxLength: 10));
            //AddColumn("dbo.Rahnemun_Users", "CellphoneNo", c => c.String(nullable: false, fixedLength: true, maxLength: 11));
            AddColumn("dbo.Rahnemun_Users", "CellphoneNo", c => c.String(nullable: true, fixedLength: true, maxLength: 11));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rahnemun_Consultants", "BankCardNo");
            DropColumn("dbo.Rahnemun_Users", "NationalId");
            DropColumn("dbo.Rahnemun_Users", "CellphoneNo");
            AddColumn("dbo.Rahnemun_Consultants", "BankCardNo", c => c.String(nullable: false, maxLength: 16));
            AddColumn("dbo.Rahnemun_Users", "NationalId", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Rahnemun_Users", "CellphoneNo", c => c.String(nullable: false, maxLength: 11));
        }
    }
}
