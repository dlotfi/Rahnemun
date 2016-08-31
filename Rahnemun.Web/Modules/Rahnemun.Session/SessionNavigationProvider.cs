using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.SessionContracts;
using Rahnemun.UIContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.Session
{
    public class SessionNavigationProvider: INavigationProvider
    {
        private readonly ISessionService _sessionService;

        public SessionNavigationProvider(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public IEnumerable<NavigationItemModel> GetNavigationItems()
        {
            yield break;
        }

        public bool TryGetSiteMapNodeInfo(SiteMapNodeModel node, out Func<UrlHelper, string> nodeRoute, out string nodeTitle, out SiteMapNodeModel parentNode)
        {
            switch (node.NavigationId)
            {
                case "StartNewSession":
                    if (node.NavigationData == null ||
                        !node.NavigationData.ContainsKey("ConsultantId") || !(node.NavigationData["ConsultantId"] is int) ||
                        !node.NavigationData.ContainsKey("CategoryId") || !(node.NavigationData["CategoryId"] is int))
                    {
                        SetDefaultValues(out nodeRoute, out nodeTitle, out parentNode);
                        return false;
                    }
                    var consultantId = (int)node.NavigationData["ConsultantId"];
                    var categoryId = (int)node.NavigationData["CategoryId"];
                    nodeRoute = url => url.Route<IStartNewSessionRoute>().Get(new StartNewSessionRouteModel { ConsultantId = consultantId, CategoryId = categoryId });
                    nodeTitle = "شروع جلسه جدید";
                    parentNode = new SiteMapNodeModel("ConsultantDisplay", new { ConsultantId = consultantId, CategoryId = categoryId });
                    return true;
                case "Session":
                    if (node.NavigationData == null || !node.NavigationData.ContainsKey("SessionId") || !(node.NavigationData["SessionId"] is int))
                    {
                        SetDefaultValues(out nodeRoute, out nodeTitle, out parentNode);
                        return false;
                    }
                    var sessionId = (int)node.NavigationData["SessionId"];
                    var session = _sessionService.GetSession(sessionId);
                    nodeRoute = url => url.Route<ISessionRoute>().Get(sessionId);
                    nodeTitle = "جلسه مشاوره";
                    parentNode = new SiteMapNodeModel("ConsultantDisplay", new { ConsultantId = session.Consultant.Id, CategoryId = session.Category.Id });
                    return true;
                default:
                    SetDefaultValues(out nodeRoute, out nodeTitle, out parentNode);
                    return false;
            }
        }

        private static void SetDefaultValues(out Func<UrlHelper, string> nodeRoute, out string nodeTitle, out SiteMapNodeModel parentNode)
        {
            nodeRoute = null;
            nodeTitle = null;
            parentNode = null;
        }
    }
}