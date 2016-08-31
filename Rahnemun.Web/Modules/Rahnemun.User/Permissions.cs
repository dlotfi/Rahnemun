using System.Collections.Generic;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Security;
using Edreamer.Framework.Security.Permissions;

namespace Rahnemun.User
{
    public class UserPermissions : IPermissionRegistrar
    {
        public static readonly Permission ConsultantFinalRegister = new Permission { Name = "ConsultantFinalRegister", Description = "ثبت اطلاعات نهایی مشاور" };
        public void RegisterPermissions(ICollection<Permission> permissions)
        {
            permissions.AddRange(new[] { ConsultantFinalRegister });
        }

        public void RegisterDefaultStereotypes(ICollection<PermissionStereotype> stereotypes)
        {
            stereotypes.AddRange(new[] {
                new PermissionStereotype {
                    RoleName = "Administrator",
                    Permissions = new[] { ConsultantFinalRegister }
                },
                new PermissionStereotype {
                    RoleName = "Consultant",
                    Permissions = new[] { ConsultantFinalRegister }
                }
            });
        }
    }
}