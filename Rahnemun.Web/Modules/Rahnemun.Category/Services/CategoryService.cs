using System.Linq;
using Rahnemun.CategoryContracts;
using Rahnemun.Domain;

namespace Rahnemun.Category.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRahnemunDataContext _dataContext;

        public CategoryService(IRahnemunDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //public IQueryable<CategoryGroupModel> CategoryGroups
        //{
        //    get
        //    {
        //        return _dataContext.CategoryGroups.Select(c => new CategoryGroupModel
        //        {
        //            Id = c.Id,
        //            Caption = c.Caption,
        //            DisplayOrder = c.DisplayOrder
        //        });
        //    }
        //}

        public IQueryable<CategoryModel> Categories
        {
            get
            {
                return _dataContext.Categories.Select(c => new CategoryModel
                {
                    Id = c.Id,
                    Caption = c.Caption,
                    Description = c.Description,
                    Terms = c.Terms,
                    DisplayOrder = c.DisplayOrder,
                    CategoryGroup = new CategoryGroupModel
                    {
                        Id = c.CategoryGroup.Id,
                        Caption = c.CategoryGroup.Caption,
                        DisplayOrder = c.CategoryGroup.DisplayOrder
                    }
                });
            }
        }

        //public CategoryGroupModel GetCategoryGroup(int id)
        //{
        //    return CategoryGroups.SingleOrDefault(c => c.Id == id);
        //}

        public CategoryModel GetCategory(int id)
        {
            return Categories.SingleOrDefault(c => c.Id == id);
        }
    }
}