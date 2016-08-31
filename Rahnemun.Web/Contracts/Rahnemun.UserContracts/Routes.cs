using Edreamer.Framework.Mvc.Routes;
using Rahnemun.Common;

namespace Rahnemun.UserContracts
{
    public interface IConsultantDisplayRoute : INamedRoute<ConsultantIdModel> { }
    public interface IConsultantFinalRegisterRoute : INamedRoute { }
    //public interface IConsultantPreliminaryRegisterRoute : INamedRoute { }
    public interface IConsultantJoinUsRoute : INamedRoute { }
    public interface IConsultantEditRoute : INamedRoute { }
    public interface IPasswordResetRoute : INamedRoute<NonceRouteModel> { }
    public interface IConfirmEmailRoute : INamedRoute<NonceRouteModel> { }
    public interface IPasswordResetRequestRoute : INamedRoute { }
    public interface IConfirmEmailRequestRoute : INamedRoute { }
    public interface ILogInRoute : INamedRoute<ReturnUrlModel> { }
    public interface ILogOutRoute : INamedRoute<ReturnUrlModel> { }
    public interface IConsulteeRegisterOrLoginRoute : INamedRoute<ReturnUrlModel> { }
    public interface IConsulteeEditRoute : INamedRoute { }
    public interface IStartNewSessionRoute : INamedRoute<StartNewSessionRouteModel> { }
    public interface IDashboardRoute : INamedRoute { }
    public interface IUnauthorizedErrorRoute : INamedRoute<UnauthorizedErrorRouteModel> { }
}
