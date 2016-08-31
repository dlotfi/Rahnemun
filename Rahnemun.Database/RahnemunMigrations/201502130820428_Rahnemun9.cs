namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_Consultants", "ConsultantNewData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rahnemun_Consultants", "ConsultantNewData");
        }
    }
}
