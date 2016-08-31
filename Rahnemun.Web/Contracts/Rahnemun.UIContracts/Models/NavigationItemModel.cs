using System.Web;
using System.Web.Mvc;

namespace Rahnemun.UIContracts
{
    public class NavigationItemModel
    {
        public string NavigationId { get; set; }
        public int Priority { get; set; }
        public bool Footer { get; set; }

        public NavigationHtmlGeneratorFunc Html { get; set; }

        public NavigationLinkGeneratorFunc Link { get; set; }
    }

    public delegate IHtmlString NavigationHtmlGeneratorFunc(HtmlHelper htmlHelper, bool active);
    public delegate Link NavigationLinkGeneratorFunc(UrlHelper urlHelper, string requestUrl);

    public class Link
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
    }
}