using Edreamer.Framework.Mvc.WebParts;

namespace Rahnemun.UserContracts
{
    public interface IUserMenuWebPart : IWebPart { }
    public interface IUserCalltoActionWebPart : IWebPart<ConsultantIdModel> { }
    public interface ILoginDialogWebPart : IWebPart { }
    public interface IConsultantEditWebPart : IWebPart { }
}
