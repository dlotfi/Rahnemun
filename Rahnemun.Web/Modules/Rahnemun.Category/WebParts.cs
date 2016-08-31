using Rahnemun.CategoryContracts;
using Edreamer.Framework.Mvc.WebParts;
using Rahnemun.UIContracts;

namespace Rahnemun.Category
{
    public class CategoryListWebPart : ActionWebPart, ICategoryListWebPart
    {
        public CategoryListWebPart() : base("Category", "CategoryListWebPart") { }
    }

    public class CategoryMenuWebPart : ActionWebPart<CategoryMenuWebPartModel>, ICategoryMenuWebPart
    {
        public CategoryMenuWebPart() : base("Category", "CategoryMenuWebPart") { }
    }
}