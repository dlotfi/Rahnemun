using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edreamer.Framework.Helpers;
using Rahnemun.Layout.Models;
using Rahnemun.UIContracts;

namespace Rahnemun.Layout.Controllers
{
    public class NavigationController : Controller
    {
        private readonly IEnumerable<INavigationProvider> _navigationProviders;

        public NavigationController(IEnumerable<INavigationProvider> navigationProviders)
        {
            _navigationProviders = navigationProviders;
        }

        [ChildActionOnly]
        public PartialViewResult NavigationWebPart(string navigationId, bool footer)
        {
            var navigationItems = _navigationProviders
                .SelectMany(p => p.GetNavigationItems())
                .Where(nv => (nv.Html != null || nv.Link != null) && nv.Footer == footer)
                .OrderByDescending(nv => nv.Priority)
                .ToList();

            foreach (var navigationItem in navigationItems)
            {
                if (navigationItem.Html == null)
                {
                    var link = navigationItem.Link(Url, Request.RawUrl);
                    navigationItem.Html = (html, active) =>
                        new HtmlString($"<a {(!footer && active ? "class='current' " : "")} title='{link.Title ?? link.Caption}' href='{link.Url}'>{link.Caption}</a>");
                }
            }

            return PartialView(new NavigationViewModel { NavigationItems = navigationItems, CurrentItemId = navigationId, Footer = footer });
        }

        [ChildActionOnly]
        public PartialViewResult BreadcrumbWebPart(string navigationId, object navigationData)
        {
            Throw.IfArgumentNullOrEmpty(navigationId, "navigationId");
            
            var breadcrumb = new List<SiteMapNodeViewModel>();
            var sitemapNode = new SiteMapNodeModel(navigationId, navigationData);
            while (sitemapNode != null)
            {
                SiteMapNodeModel parentNode = null;
                Func<UrlHelper, string> route = null;
                string title = null;
                foreach (var provider in _navigationProviders)
                {
                    if (provider.TryGetSiteMapNodeInfo(sitemapNode, out route, out title, out parentNode)) break;
                }
                breadcrumb.Add(new SiteMapNodeViewModel
                               {
                                   Title = title ?? "",
                                   Description = title ?? "",
                                   Url = route != null ? route(Url) : ""                                  
                               });
                sitemapNode = parentNode;
            }
            breadcrumb.Reverse();

            return PartialView(breadcrumb);
        }
    }
}
