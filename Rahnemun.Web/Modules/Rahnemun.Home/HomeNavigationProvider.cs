using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.HomeContracts;
using Rahnemun.UIContracts;

namespace Rahnemun.Home
{
    public class HomeNavigationProvider: INavigationProvider
    {
        public IEnumerable<NavigationItemModel> GetNavigationItems()
        {
            yield return new NavigationItemModel
            {
                NavigationId = "Home",
                Link = (url, requestUrl) => new Link { Url = "/", Caption = "صفحه اصلی" },
                Priority = 1000
            };
            yield return new NavigationItemModel
            {
                NavigationId = "HowItWorks",
                Link = (url, requestUrl) => new Link {Url = url.Route<IHowItWorksRoute>().Get(), Caption = "چرا رهنمون؟" },
                Priority = 900
            };

            yield return new NavigationItemModel
            {
                NavigationId = "Terms",
                Link = (url, requestUrl) => new Link { Url = url.Route<ITermsRoute>().Get(), Caption = "قوانین", Title = "قوانین و شرایط استفاده از رهنمون" },
                Priority = 600,
                Footer = true
            };
            yield return new NavigationItemModel
            {
                NavigationId = "Privacy",
                Link = (url, requestUrl) => new Link { Url = url.Route<IPrivacyRoute>().Get(), Caption = "حریم خصوصی", Title = "سیاست‌های حریم خصوصی" },
                Priority = 500,
                Footer = true
            };
            yield return new NavigationItemModel
            {
                NavigationId = "About",
                Link = (url, requestUrl) => new Link { Url = url.Route<IAboutRoute>().Get(), Caption = "درباره رهنمون", Title = "درباره رهنمون" },
                Priority = 200,
                Footer = true
            };
        }

        public bool TryGetSiteMapNodeInfo(SiteMapNodeModel node, out Func<UrlHelper, string> nodeRoute, out string nodeTitle, out SiteMapNodeModel parentNode)
        {
            switch (node.NavigationId)
            {
                case "Home":
                    nodeRoute = url => "/";
                    nodeTitle = "خانه";
                    parentNode = null;
                    return true;
                case "HowItWorks":
                    nodeRoute = url => url.Route<IHowItWorksRoute>().Get();
                    nodeTitle = "چرا رهنمون؟";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "About":
                    nodeRoute = url => url.Route<IAboutRoute>().Get();
                    nodeTitle = "درباره رهنمون";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "Privacy":
                    nodeRoute = url => url.Route<IPrivacyRoute>().Get();
                    nodeTitle = "سیاست‌های حریم خصوصی";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "Terms":
                    nodeRoute = url => url.Route<ITermsRoute>().Get();
                    nodeTitle = "قوانین";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "Error":
                    nodeRoute = null;
                    nodeTitle = "خطا";
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