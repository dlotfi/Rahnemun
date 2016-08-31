using System.Collections.Generic;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Security;
using Edreamer.Framework.Security.Permissions;

namespace Rahnemun.Category
{
    public class CategoryPermissions : IPermissionRegistrar
    {
        public static readonly Permission CategoryDisplay = new Permission { Name = "CategoryDisplay", Description = "نمایش گروه ها"};
        
        public void RegisterPermissions(ICollection<Permission> permissions)
        {
            permissions.AddRange(new[] {CategoryDisplay});
        }

        public void RegisterDefaultStereotypes(ICollection<PermissionStereotype> stereotypes)
        {
            stereotypes.AddRange(new[] {
                new PermissionStereotype {
                    RoleName = "Administrator",
                    Permissions = new[] {CategoryDisplay}
                },
                new PermissionStereotype {
                    RoleName = "Authenticated",
                    Permissions = new[] { CategoryDisplay }
                },
                new PermissionStereotype {
                    RoleName = "Anonymous",
                    Permissions = new[] { CategoryDisplay }
                }
            });
        }
    }
}