namespace Rahnemun.Database.FrameworkMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Framework2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Framework_Users", "UserData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Framework_Users", "UserData");
        }
    }
}
