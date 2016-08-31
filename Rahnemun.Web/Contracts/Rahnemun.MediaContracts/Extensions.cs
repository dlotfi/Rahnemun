using System.Web;
using System.Web.Mvc;
using Edreamer.Framework.Mvc.Extensions;

namespace Rahnemun.MediaContracts
{
    public static class Extensions
    {
        public static IHtmlString Image(this HtmlHelper htmlHelper, int? mediaId, string description, ImageSize? size = null, bool maxFit = false, bool includeSize = false, string defaultImageResourceName = null, string defaultImagePath = null)
        {
            return htmlHelper.WebPart<IImageWebPart>().Get(new ImageWebPartModel
                                                               {
                                                                   Id = mediaId,
                                                                   Description = description,
                                                                   Size = size,
                                                                   MaxFit = maxFit,
                                                                   IncludeSize = includeSize,
                                                                   DefaultImagePath = defaultImagePath,
                                                                   DefaultImageResourceName = defaultImageResourceName
                                                               });
        }
    }
}
