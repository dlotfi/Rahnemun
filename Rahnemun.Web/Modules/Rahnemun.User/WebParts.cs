using Edreamer.Framework.Composition;
using Edreamer.Framework.Mvc.WebParts;
using Rahnemun.UIContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.User
{
    public class UserBarWebPart : ActionWebPart<UserBarWebPartModel>, IUserBarWebPart
    {
        public UserBarWebPart() : base("Dashboard", "UserBarWebPart") { }
    }

    [PartPriority(PartPriorityAttribute.Minimum)]
    public class UserCalltoActionWebPart : ActionWebPart<ConsultantIdModel>, IUserCalltoActionWebPart
    {
        public UserCalltoActionWebPart() : base("Consultant", "UserCalltoActionWebPart") { }
    }

    public class ConsultantEditWebPart : ActionWebPart, IConsultantEditWebPart
    {
        public ConsultantEditWebPart() : base("Consultant", "ConsultantEditWebPart") { }
    }

    public class LoginDialogWebPart : SimpleWebPart, ILoginDialogWebPart
    {
        public LoginDialogWebPart() : base("LoginDialogWebPart") { }
    }
}