using System.Collections.Generic;
using Rahnemun.UserContracts;

namespace Rahnemun.User.Models
{
    public class DashboardViewModel
    {
        public string CurrentUsername { get; set; }
        public IEnumerable<DashboardItemModel> DashboardItems { get; set; }
    }
}