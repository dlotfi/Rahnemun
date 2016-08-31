using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.CategoryContracts;
using Rahnemun.UIContracts;

namespace Rahnemun.Category
{
    public class CategoryNavigationProvider: INavigationProvider
    {
        public IEnumerable<NavigationItemModel> GetNavigationItems()
        {
            yield return new NavigationItemModel
                         {
                             NavigationId = "Category",
                             Html = (html, active) => html.WebPart<ICategoryMenuWebPart>().Get(active),
                             Priority = 500
                         };
        }

        public bool TryGetSiteMapNodeInfo(SiteMapNodeModel node, out Func<UrlHelper, string> nodeRoute, out string nodeTitle, out SiteMapNodeModel parentNode)
        {
            if (node.NavigationId == "Category")
            {
                nodeRoute = url => url.Route<ICategoriesListRoute>().Get();
                nodeTitle = "گروه های مشاوره";
                parentNode = new SiteMapNodeModel("Home", null);
                return true;
            }

            nodeRoute = null;
            nodeTitle = null;
            parentNode = null;
            return false;
        }
    }
}