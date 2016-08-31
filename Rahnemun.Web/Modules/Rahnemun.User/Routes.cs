using System.Web.Mvc;
using Edreamer.Framework.Caching;
using Edreamer.Framework.Composition;
using Edreamer.Framework.Mvc.Routes;
using Rahnemun.CategoryContracts;
using Rahnemun.Common;
using Rahnemun.UserContracts;

namespace Rahnemun.User
{
    public class CategoryDetailsRoute : NamedRoute<CategoryIdModel>, ICategoryDetailsRoute { }
    public class ConsultantDisplayRoute : NamedRoute<ConsultantIdModel>, IConsultantDisplayRoute { }
    public class ConsultantFinalRegisterRoute : NamedRoute , IConsultantFinalRegisterRoute { }
    //public class ConsultantPreliminaryRegisterRoute : NamedRoute, IConsultantPreliminaryRegisterRoute { }
    public class ConsultantJoinUsRoute : NamedRoute, IConsultantJoinUsRoute { }
    public class ConsultantEditRoute : NamedRoute, IConsultantEditRoute  { }
    public class ConfirmEmailRoute : NamedRoute<NonceRouteModel>, IConfirmEmailRoute { }
    public class PasswordResetRoute : NamedRoute<NonceRouteModel>, IPasswordResetRoute { }
    public class ConfirmEmailRequestRoute : NamedRoute, IConfirmEmailRequestRoute { }
    public class PasswordResetRequestRoute : NamedRoute, IPasswordResetRequestRoute { }
    public class LogInRoute : NamedRoute<ReturnUrlModel>, ILogInRoute { }
    public class LogOutRoute : NamedRoute<ReturnUrlModel>, ILogOutRoute { }
    public class ConsulteeRegisterOrLoginRoute : NamedRoute<ReturnUrlModel>, IConsulteeRegisterOrLoginRoute { }
    public class VoidConsulteeEditRoute : VoidNamedRoute, IConsulteeEditRoute { }
    public class VoidStartNewSessionRoute : VoidNamedRoute<StartNewSessionRouteModel>, IStartNewSessionRoute { }
    public class DashboardRoute : NamedRoute, IDashboardRoute { }
    public class UnauthorizedErrorRoute : NamedRoute<UnauthorizedErrorRouteModel>, IUnauthorizedErrorRoute { }

    public class ConsultantRouteRegistrar : IRouteRegistrar
    {
        private readonly ICompositionContainerAccessor _compositionContainerAccessor;
        private readonly IUserService _userService;
        private readonly ICacheFactory _cacheFactory;

        public ConsultantRouteRegistrar(ICompositionContainerAccessor compositionContainerAccessor, IUserService userService, ICacheFactory cacheFactory)
        {
            _compositionContainerAccessor = compositionContainerAccessor;
            _userService = userService;
            _cacheFactory = cacheFactory;
        }

        public void RegisterRoutes(RouteRegistrarContext context)
        {
            //context.MapRoute<ConsultantPreliminaryRegisterRoute>("consultant/register", new { Controller = "Consultant", Action = "PreliminaryRegister" });
            context.MapRoute<ConsultantJoinUsRoute>("join", new { Controller = "Consultant", Action = "JoinUs" });
            context.MapRoute<CategoryDetailsRoute>("category/{CategoryId}/{CategorySlug}", new { Controller = "Consultant", Action = "Index", CategorySlug = UrlParameter.Optional },
                null, new CategoryDetiailsRouteCanonicalizer(_compositionContainerAccessor, _cacheFactory, "CategoryId", "CategorySlug"));
            context.MapRoute<ConsultantFinalRegisterRoute>("consultant/final-register", new { Controller = "Consultant", Action = "FinalRegister" });
            context.MapRoute<ConsultantEditRoute>("consultant/edit", new { Controller = "Consultant", Action = "Edit" });
            context.MapRoute<ConsultantDisplayRoute>("consultant/{ConsultantId}/{ConsultantNameSlug}", new { Controller = "Consultant", Action = "Display", ConsultantNameSlug = UrlParameter.Optional },
                new { ConsultantId = @"\d+" }/*conflicts with finalRegister route*/, new ConsultantDisplayRouteCanonicalizer(_userService, "ConsultantId", "ConsultantNameSlug")); 

            context.MapRoute<ConfirmEmailRoute>("account/confirm-email", new { Controller = "Account", Action = "ConfirmEmail" });
            context.MapRoute<PasswordResetRoute>("account/password-reset", new { Controller = "Account", Action = "PasswordReset" });
            context.MapRoute<ConfirmEmailRequestRoute>("account/request-confirm-email", new { Controller = "Account", Action = "RequestConfirmEmail" });
            context.MapRoute<PasswordResetRequestRoute>("account/request-password-reset", new { Controller = "Account", Action = "RequestPasswordReset" });
            context.MapRoute<LogInRoute>("account/login", new { Controller = "Account", Action = "Login" });
            context.MapRoute<LogOutRoute>("account/logout", new { Controller = "Account", Action = "Logout" });
            context.MapRoute<UnauthorizedErrorRoute>("account/unauthorized", new { Controller = "Account", Action = "Unauthorized" });
            
            context.MapRoute<ConsulteeRegisterOrLoginRoute>("consultee/register-login", new { Controller = "Consultee", Action = "RegisterOrLogin" });
            context.MapRoute("consultee/register", new { Controller = "Consultee", Action = "Register" });
            context.MapRoute("consultee/login", new { Controller = "Consultee", Action = "Login" });

            context.MapRoute<DashboardRoute>("dashboard", new { Controller = "Dashboard", Action = "Dashboard" });
        }
    }
}