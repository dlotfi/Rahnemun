using System.Collections.Generic;
using Edreamer.Framework.Context;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.Security;
using Rahnemun.UserContracts;

namespace Rahnemun.User
{
    public class UserDashboardItemProvider: IDashboardItemProvider
    {
        private readonly IConsultantService _consultantService;
        private readonly IWorkContextAccessor _workContextAccessor;

        public UserDashboardItemProvider(IConsultantService consultantService, IWorkContextAccessor workContextAccessor)
        {
            _consultantService = consultantService;
            _workContextAccessor = workContextAccessor;
        }

        public IEnumerable<DashboardItemModel> GetDashboardItems()
        {
            var currentUser = _workContextAccessor.Context.CurrentUser();
            if (!_consultantService.IsConsultant(currentUser.Id) || currentUser.IsPreliminaryRegisteredConsultant()) yield break;
            
            yield return new DashboardItemModel
                {
                    Title = "ویرایش پروفایل",
                    Priority = 800,
                    Html = html => html.WebPart<IConsultantEditWebPart>().Get()
            };
        }
    }
}