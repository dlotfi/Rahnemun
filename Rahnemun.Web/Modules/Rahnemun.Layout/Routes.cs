using Edreamer.Framework.Mvc.Routes;

namespace Rahnemun.Layout
{
    public class LayoutRouteRegistrar : IRouteRegistrar
    {
        public void RegisterRoutes(RouteRegistrarContext context)
        {
            context.MapRoute("dialog", new { Controller = "Dialog", Action = "GetDialog" });
        }
    }
}