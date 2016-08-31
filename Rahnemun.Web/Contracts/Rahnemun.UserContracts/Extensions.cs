using System.Web;
using System.Web.Mvc;
using Rahnemun.Common;
using Rahnemun.MediaContracts;

namespace Rahnemun.UserContracts
{
    public static class Extensions
    {
        public static IHtmlString ProfilePicture(this HtmlHelper htmlHelper, int? profilePictureId, Gender? gender, string description, ImageSize? size, bool maxFit = false)
        {
            string defaultResourceName;
            switch (gender)
            {
                case Gender.Male: defaultResourceName = "DefaultMaleProfilePicture"; break;
                case Gender.Female: defaultResourceName = "DefaultFemaleProfilePicture"; break;
                default: defaultResourceName = "DefaultUnknownProfilePicture"; break;
            }
            return htmlHelper.Image(profilePictureId, description, size, maxFit, false, defaultResourceName);
        }
    }
}
