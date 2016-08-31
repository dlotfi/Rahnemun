using System.Web.Mvc;
using Edreamer.Framework.Helpers;

namespace Rahnemun.Common
{
    public static class MvcHelperExtensions
    {
        public static UrlHelper GetUrlHelper(this HtmlHelper html)
        {
            Throw.IfArgumentNull(html, "html");
            return new UrlHelper(html.ViewContext.RequestContext);
        }

        public static bool MarkFieldAsRequired(this HtmlHelper htmlHelper)
        {
            var viewData = htmlHelper.ViewData;
            var metadata = viewData.ModelMetadata;
            var containerMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, metadata.ContainerType);
            var markRequiredFields = containerMetadata.AdditionalValues.ContainsKey("MarkRequiredFields") && (bool)containerMetadata.AdditionalValues["MarkRequiredFields"];
            var isRequired = metadata.IsRequired || viewData["IsRequired"] != null && (bool)viewData["IsRequired"];
            return isRequired && markRequiredFields;
        }
    }
}