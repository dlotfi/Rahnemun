using System.Collections.Generic;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.SessionContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.Session
{
    public class SessionDashboardItemProvider: IDashboardItemProvider
    {
       public IEnumerable<DashboardItemModel> GetDashboardItems()
        {
            yield return new DashboardItemModel
                {
                    Title = "جلسات خاتمه نیافته",
                    Priority = 1000,
                    Html = html => html.WebPart<IActiveSessionsWebPart>().Get()
                };

            yield return new DashboardItemModel
                {
                    Title = "جلسات خاتمه یافته",
                    Priority = 900,
                    Html = html => html.WebPart<IInactiveSessionsWebPart>().Get()
                };
        }
    }
}