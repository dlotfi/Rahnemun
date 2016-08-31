namespace Rahnemun.Database.RahnemunMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rahnemun14 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rahnemun_CustomerMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        FullName = c.String(maxLength: 30),
                        Email = c.String(maxLength: 255),
                        Subject = c.Byte(nullable: false),
                        Message = c.String(nullable: false, maxLength: 2000),
                        UserAgent = c.String(nullable: false, maxLength: 512),
                        SentTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Rahnemun_CustomerMessages");
        }
    }
}
