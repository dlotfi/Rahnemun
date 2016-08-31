using System.Collections.Generic;
using System.Data.Entity.Migrations;
using Edreamer.Framework.Domain;
using Edreamer.Framework.Security;
using Role = Edreamer.Framework.Domain.Role;
using User = Edreamer.Framework.Domain.User;

namespace Rahnemun.Database.FrameworkMigrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<FrameworkDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"FrameworkMigrations";
        }

        protected override void Seed(FrameworkDataContext context)
        {
            var roles = new List<Role>
            {
                    new Role { Name = "Administrator", DisplayName="مدیر سیستم" },
                    new Role { Name = "Anonymous", DisplayName="کاربر وارد نشده" },
                    new Role { Name = "Authenticated", DisplayName="کاربر وارد شده" }
            };
            context.Set<Role>().AddOrUpdate(r => r.Name, roles.ToArray());

            var admin = new User
            {
                Username = "admin",
                Email = "admin@rahnemun.com",
                Approved = true,
                EmailConfirmed = true,
                PasswordFormat = PasswordFormat.Hashed,
                HashAlgorithm = "SHA1",
                Password = "7YNP9wO299s3vfG71utHHcqDSAM=",
                PasswordSalt = "TUhhs2qU1zt4ykEd/DOIgQ=="
            };
            admin.Roles.Add(roles[0]);
            context.Set<User>().AddOrUpdate(u => u.Username, admin);

            base.Seed(context);
        }
    }
}
