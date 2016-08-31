using Edreamer.Framework.Mvc.Routes;

namespace Rahnemun.CategoryContracts
{
    public interface ICategoriesListRoute : INamedRoute { }

    public interface ICategoryDetailsRoute : INamedRoute<CategoryIdModel> { }
}
