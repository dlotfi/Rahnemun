using System.Linq;
using Edreamer.Framework.Composition;

namespace Rahnemun.CategoryContracts
{
    [InterfaceExport]
    public interface ICategoryService
    {
        //IQueryable<CategoryGroupModel> CategoryGroups { get; }
        IQueryable<CategoryModel> Categories { get; }

        //CategoryGroupModel GetCategoryGroup(int id);
        CategoryModel GetCategory(int id);
    }
}
