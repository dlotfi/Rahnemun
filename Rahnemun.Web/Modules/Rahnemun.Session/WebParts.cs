using Edreamer.Framework.Composition;
using Edreamer.Framework.Mvc.WebParts;
using Rahnemun.SessionContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.Session
{
    public class ActiveSessionsWebPart : ActionWebPart, IActiveSessionsWebPart
    {
        public ActiveSessionsWebPart() : base("Session", "ActiveSessionsWebPart") { }
    }

    public class InactiveSessionsWebPart : ActionWebPart, IInactiveSessionsWebPart
    {
        public InactiveSessionsWebPart() : base("Session", "InactiveSessionsWebPart") { }
    }

    [PartPriority(PartPriorityAttribute.Maximum)]
    public class UserCalltoActionWebPart : ActionWebPart<ConsultantIdModel>, IUserCalltoActionWebPart
    {
        public UserCalltoActionWebPart() : base("Session", "UserCalltoActionWebPart") { }
    }
}