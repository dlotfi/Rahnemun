using System.Web.Mvc;
using Edreamer.Framework.Mvc.Routes;
using Rahnemun.MediaContracts;

namespace Rahnemun.Media
{
    public class ImageRoute : NamedRoute<ImageRouteModel>, IImageRoute { }
   
    public class MediaRouteRegistrar : IRouteRegistrar
    {
        public void RegisterRoutes(RouteRegistrarContext context)
        {
            context.MapRoute<ImageRoute>("image/{Id}", new { Controller = "Media", Action = "Image", Id = UrlParameter.Optional });
        }
    }
}