using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.BlogContracts;
using Rahnemun.UIContracts;

namespace Rahnemun.Blog
{
    public class BlogNavigationProvider: INavigationProvider
    {
        public IEnumerable<NavigationItemModel> GetNavigationItems()
        {
            yield return new NavigationItemModel
            {
                NavigationId = "Blog",
                Link = (url, requestUrl) => new Link { Url = url.Route<IBlogRoute>().Get(null), Caption = "وبلاگ" },
                Priority = 400
            };
        }

        public bool TryGetSiteMapNodeInfo(SiteMapNodeModel node, out Func<UrlHelper, string> nodeRoute, out string nodeTitle, out SiteMapNodeModel parentNode)
        {
            if (node.NavigationId == "Blog")
            {
                nodeRoute = url => url.Route<IBlogRoute>().Get(null);
                nodeTitle = "وبلاگ";
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