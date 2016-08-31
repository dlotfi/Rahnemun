using Edreamer.Framework.Bootstrapping;
using Edreamer.Framework.Composition;

namespace Rahnemun.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // ReSharper disable InconsistentNaming
        protected void Application_Start()
        {
            Sarter.Run(new CompositionContainerAccessor(new[] { "~/bin", "~/Modules" }));
        }
    }
}