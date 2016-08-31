using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.ContactUsContracts;
using Rahnemun.UIContracts;

namespace Rahnemun.ContactUs
{
    public class ContactUsNavigationProvider : INavigationProvider
    {
        public IEnumerable<NavigationItemModel> GetNavigationItems()
        {
            yield return new NavigationItemModel
            {
                NavigationId = "ContactUs",
                Link = (url, requestUrl) => new Link {Url = url.Route<IContactUsRoute>().Get(), Caption = "تماس با ما"},
                Priority = 100,
                Footer = true
            };
        }

        public bool TryGetSiteMapNodeInfo(SiteMapNodeModel node, out Func<UrlHelper, string> nodeRoute, out string nodeTitle, out SiteMapNodeModel parentNode)
        {
            switch (node.NavigationId)
            {
                case "ContactUs":
                    nodeRoute = url => url.Route<IContactUsRoute>().Get();
                    nodeTitle = "تماس با ما";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                default:
                    nodeRoute = null;
                    nodeTitle = null;
                    parentNode = null;
                    return false;
            }
        }
    }
}