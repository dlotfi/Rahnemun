using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Edreamer.Framework.Context;
using Edreamer.Framework.Security;
using Rahnemun.User.Models;
using Rahnemun.UserContracts;

namespace Rahnemun.User.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IEnumerable<IDashboardItemProvider> _providers;
        private readonly IUserService _userService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IUserNotificationProvider _userNotificationProvider;

        public DashboardController(IEnumerable<IDashboardItemProvider> providers, IUserService userService, 
            IWorkContextAccessor workContextAccessor, IUserNotificationProvider userNotificationProvider)
        {
            _providers = providers;
            _userService = userService;
            _workContextAccessor = workContextAccessor;
            _userNotificationProvider = userNotificationProvider;
        }

        // GET: /dashboard
        [HttpGet]
        public ActionResult Dashboard()
        {
            var currentUser = _workContextAccessor.Context.CurrentUser();
            if (currentUser == null)
                return new HttpUnauthorizedResult();

            ConsultantPreliminaryData currentUserData = null;
            if (currentUser.IsPreliminaryRegisteredConsultant())
                currentUserData = (ConsultantPreliminaryData)currentUser.UserData;

            var dashboardItems = _providers
                .SelectMany(p => p.GetDashboardItems())
                .OrderByDescending(i => i.Priority)
                .ToList();

            return View("Dashboard", new DashboardViewModel
            {
                CurrentUsername = currentUserData != null
                                  ? currentUser.GetPreliminaryRegisteredConsultantFullName()
                                  : _userService.GetUserFullName(_userService.GetUser(currentUser.Id)),
                DashboardItems = dashboardItems
            });
        }

        [ChildActionOnly]
        public PartialViewResult UserBarWebPart(bool responsiveAlternative)
        {
            var currentUser = _workContextAccessor.Context.CurrentUser();

            ConsultantPreliminaryData currentUserData = null;
            if(currentUser != null && currentUser.IsPreliminaryRegisteredConsultant())
                currentUserData = (ConsultantPreliminaryData)currentUser.UserData;

            return PartialView(new UserBarWebPartViewModel
            {
                Notification = currentUser != null ? _userNotificationProvider.GetUserNotification(currentUser.Id) : null,
                ResponsiveAlternative = responsiveAlternative,
                UserLoggedIn = currentUser != null,
                Name = currentUser == null 
                    ? String.Empty 
                    : currentUserData != null
                       ? currentUser.GetPreliminaryRegisteredConsultantFullName()
                       : _userService.GetUserFullName(_userService.GetUser(currentUser.Id))
            });
        }
    }
}