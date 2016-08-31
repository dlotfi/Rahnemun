using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Security;
using Rahnemun.User.Models;

namespace Rahnemun.User
{
    public static class Extensions
    {
        public static bool IsPreliminaryRegisteredConsultant(this Edreamer.Framework.Security.User userAccount)
        {
            return userAccount.UserData is ConsultantPreliminaryData;
        }

        public static string GetPreliminaryRegisteredConsultantFullName(this Edreamer.Framework.Security.User userAccount)
        {
            Throw.IfArgumentNull(userAccount, "userAccount");
            Throw.IfNot(userAccount.UserData is ConsultantPreliminaryData)
                .AnArgumentException("User should be a preliminary registerd consultant.", "userAccount");
            var userData = (ConsultantPreliminaryData)userAccount.UserData;
            return userData.FirstName + " " + userData.LastName;
        }

        public static void AddPasswordValidationError(this ModelStateDictionary modelState, MembershipSettings membershipSettings, string password, string modelName)
        {
            Throw.IfArgumentNull(modelState, "modelState");
            Throw.IfArgumentNull(modelName, "modelName");

            if (password == null || membershipSettings == null) return;

            if (password.Length < membershipSettings.MinRequiredPasswordLength)
            {
                modelState.AddModelError(modelName, "داشتن حداقل {0} حرف برای کلمه عبور الزامی است."
                   .FormatWith(membershipSettings.MinRequiredPasswordLength));

            }
            else if (password.Count(c => !Char.IsLetterOrDigit(c)) < membershipSettings.MinRequiredNonAlphanumericCharacters)
            {
                modelState.AddModelError(modelName, "داشتن حداقل {0} حرف غیر الفبایی و عددی برای کلمه عبور الزامی است."
                    .FormatWith(membershipSettings.MinRequiredNonAlphanumericCharacters));
            }
            else if (!String.IsNullOrEmpty(membershipSettings.PasswordStrengthRegularExpression) &&
                !new Regex(membershipSettings.PasswordStrengthRegularExpression).IsMatch(password))
            {
                modelState.AddModelError(modelName, "کلمه عبور فاقد الگوی تعریف شده است.");
            }
        }
    }
}