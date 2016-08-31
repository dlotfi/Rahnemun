using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Edreamer.Framework.Composition;

namespace Rahnemun.UIContracts
{
    [InterfaceExport]
    public interface INavigationProvider
    {
        IEnumerable<NavigationItemModel> GetNavigationItems();

        bool TryGetSiteMapNodeInfo(SiteMapNodeModel node, out Func<UrlHelper, string> nodeRoute, out string nodeTitle, out SiteMapNodeModel parentNode);
    }
}