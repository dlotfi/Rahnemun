using System.Collections.Generic;
using Edreamer.Framework.Composition;

namespace Rahnemun.UserContracts
{
    [InterfaceExport]
    public interface IDashboardItemProvider
    {
        IEnumerable<DashboardItemModel> GetDashboardItems();
    }
}
