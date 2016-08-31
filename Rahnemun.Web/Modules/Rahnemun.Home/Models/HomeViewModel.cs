using System.Collections.Generic;
using Rahnemun.HomeContracts;

namespace Rahnemun.Home.Models
{
    public class HomeViewModel
    {
        public IEnumerable<HomeSlideWidgetModel> Slides { get; set; }
        public IEnumerable<HomeWidgetModel> Intros { get; set; }
        public IEnumerable<HomeWidgetModel> Widgets { get; set; }
    }
}