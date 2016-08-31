using Edreamer.Framework.Mvc.Routes;
using Rahnemun.Common;
using Rahnemun.SessionContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.Session
{
    public class StartNewSessionRoute : NamedRoute<StartNewSessionRouteModel>, IStartNewSessionRoute { }
    public class SessionRoute : NamedRoute<IdModel>, ISessionRoute { }
    public class GetMessageAttachmentRoute : NamedRoute<IdModel>, IGetMessageAttachmentRoute { }
   
    public class SessionRouteRegistrar : IRouteRegistrar
    {
        public void RegisterRoutes(RouteRegistrarContext context)
        {
            context.MapRoute<StartNewSessionRoute>("session/new-session", new { Controller = "Session", Action = "StartNewSession" });
            context.MapRoute<SessionRoute>("session/{Id}", new { Controller = "Session", Action = "Conversation" });
            context.MapRoute<GetMessageAttachmentRoute>("message/{Id}/attachment", new { Controller = "Session", Action = "GetMessageAttachment" });
            context.MapRoute("session/{Id}/messages", new { Controller = "Session", Action = "GetMessages" });
            context.MapRoute("session/{Id}/send-message", new { Controller = "Session", Action = "SendMessage" });
            context.MapRoute("session/{Id}/set-seen", new { Controller = "Session", Action = "SetSeen" });
            context.MapRoute("session/{Id}/stop", new { Controller = "Session", Action = "Stop" });
        }
    }
}