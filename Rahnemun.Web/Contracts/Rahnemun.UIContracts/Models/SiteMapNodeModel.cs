using System.Collections.Generic;
using System.Web.Mvc;
using Edreamer.Framework.Helpers;

namespace Rahnemun.UIContracts
{
    public class SiteMapNodeModel
    {
        public SiteMapNodeModel(string navigationId, object navigationData)
        {
            Throw.IfArgumentNullOrEmpty(navigationId, "navigationId");
            NavigationId = navigationId;
            NavigationData = HtmlHelper.AnonymousObjectToHtmlAttributes(navigationData);
        }

        public string NavigationId { get; private set; }
        public IDictionary<string, object> NavigationData { get; private set; }
    }
}