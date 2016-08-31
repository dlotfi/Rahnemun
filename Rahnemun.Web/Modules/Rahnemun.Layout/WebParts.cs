using Rahnemun.UIContracts;
using Edreamer.Framework.Mvc.WebParts;

namespace Rahnemun.Layout
{
    public class NavigationWebPart : ActionWebPart<NavigationWebPartModel>, INavigationWebPart
    {
        public NavigationWebPart() : base("Navigation", "NavigationWebPart") { }
    }

    public class BreadcrumbWebPart : ActionWebPart<BreadcrumbWebPartModel>, IBreadcrumbWebPart
    {
        public BreadcrumbWebPart() : base("Navigation", "BreadcrumbWebPart") { }
    }
    
    public class DialogsWebPart : ActionWebPart, IDialogsWebPart
    {
        public DialogsWebPart() : base("Dialog", "DialogsWebPart") { }
    }
}