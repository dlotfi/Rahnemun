namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rahnemun_BlogPosts", "Subtitle", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rahnemun_BlogPosts", "Subtitle");
        }
    }
}
