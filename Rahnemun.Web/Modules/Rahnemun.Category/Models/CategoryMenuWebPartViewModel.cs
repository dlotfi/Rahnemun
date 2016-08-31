using System.Collections.Generic;
using Rahnemun.UIContracts;

namespace Rahnemun.Category.Models
{
    public class CategoryMenuWebPartViewModel: CategoryMenuWebPartModel
    {
        public IEnumerable<CategoryMenuItemViewModel> MenuItems { get; set; }
    }

    public class CategoryMenuItemViewModel
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public IEnumerable<CategoryMenuItemViewModel> SubItems { get; set; }
    }
}