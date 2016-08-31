using System.Collections.Generic;
using Rahnemun.UIContracts;

namespace Rahnemun.Layout.Models
{
    public class NavigationViewModel
    {
        public IEnumerable<NavigationItemModel> NavigationItems { get; set; }
        public string CurrentItemId { get; set; }
        public bool Footer { get; set; }
    }
}