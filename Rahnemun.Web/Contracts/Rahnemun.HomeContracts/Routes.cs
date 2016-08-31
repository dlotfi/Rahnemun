using Edreamer.Framework.Mvc.Routes;

namespace Rahnemun.HomeContracts
{
    public interface IAboutRoute : INamedRoute { }
    public interface IHowItWorksRoute : INamedRoute { }
    public interface IPrivacyRoute : INamedRoute { }
    public interface ITermsRoute : INamedRoute { }

    public interface IGeneralErrorRoute : INamedRoute<GeneralErrorRouteModel> { }
}
