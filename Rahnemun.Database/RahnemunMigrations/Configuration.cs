using System.Data.Entity.Migrations;
using Rahnemun.Domain;

namespace Rahnemun.Database.RahnemunMigrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<RahnemunDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"RahnemunMigrations";
        }
    }
}
