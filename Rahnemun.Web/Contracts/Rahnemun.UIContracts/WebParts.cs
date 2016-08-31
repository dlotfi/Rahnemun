using Edreamer.Framework.Mvc.WebParts;

namespace Rahnemun.UIContracts
{
    public interface INavigationWebPart : IWebPart<NavigationWebPartModel> { }
    public interface IUserBarWebPart : IWebPart<UserBarWebPartModel> { }
    public interface IBreadcrumbWebPart : IWebPart<BreadcrumbWebPartModel> { }
    public interface IFeedbackWebPart : IWebPart { }
    public interface IDialogsWebPart : IWebPart { }
}




