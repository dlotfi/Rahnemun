using Edreamer.Framework.Mvc.Routes;
using Rahnemun.CategoryContracts;


namespace Rahnemun.Category
{
    public class CategoriesListRoute : NamedRoute, ICategoriesListRoute { }
    public class VoidCategoryDetailsRoute : VoidNamedRoute<CategoryIdModel>, ICategoryDetailsRoute { }

    public class CategoryRouteRegistrar : IRouteRegistrar
    {
        public void RegisterRoutes(RouteRegistrarContext context)
        {
            context.MapRoute<CategoriesListRoute>("categories", new { Controller = "Category", Action = "Index" });
        }
    }
}