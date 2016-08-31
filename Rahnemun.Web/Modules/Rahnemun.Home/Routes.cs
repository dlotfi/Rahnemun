using Edreamer.Framework.Mvc.Routes;
using Rahnemun.HomeContracts;

namespace Rahnemun.Home
{
    public class HowItWorksRoute : NamedRoute, IHowItWorksRoute { }
    public class AboutRoute : NamedRoute, IAboutRoute { }
    public class PrivacyRoute : NamedRoute, IPrivacyRoute { }
    public class TermsRoute : NamedRoute, ITermsRoute { }
    public class GeneralErrorRoute : NamedRoute<GeneralErrorRouteModel>, IGeneralErrorRoute { }

    public class HomeRouteRegistrar : IRouteRegistrar
    {
        public void RegisterRoutes(RouteRegistrarContext context)
        {
            context.MapRoute("", new { Controller = "Home", Action = "Index" });
            context.MapRoute<HowItWorksRoute>("howitworks", new { Controller = "Home", Action = "HowItWorks" });
            context.MapRoute<AboutRoute>("about", new { Controller = "Home", Action = "About" });
            context.MapRoute<PrivacyRoute>("privacy", new { Controller = "Home", Action = "Privacy" });
            context.MapRoute<TermsRoute>("terms", new { Controller = "Home", Action = "Terms" });
            context.MapRoute<GeneralErrorRoute>("error", new { Controller = "Home", Action = "Error" });
        }
    }
}