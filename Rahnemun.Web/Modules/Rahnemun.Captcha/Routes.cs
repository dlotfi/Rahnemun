using Edreamer.Framework.Mvc.Routes;
using Rahnemun.CaptchaContracts;

namespace Rahnemun.Captcha
{
    public class CaptchaImageRoute : NamedRoute<CaptchaRouteModel>, ICaptchaImageRoute { }
   
    public class CaptchaRouteRegistrar : IRouteRegistrar
    {
        public void RegisterRoutes(RouteRegistrarContext context)
        {
            context.MapRoute<CaptchaImageRoute>("captcha/{Id}", new { Controller = "Captcha", Action = "Image" });
        }
    }
}