using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Edreamer.Framework.Injection;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.Category.Models;
using Rahnemun.CategoryContracts;

namespace Rahnemun.Category.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: /categories
        public ActionResult Index()
        {
            return View("CategoryIndex");
        }

        [ChildActionOnly]
        public ActionResult CategoryListWebPart()
        {
            var categories = _categoryService.Categories
                .OrderBy(c => c.CategoryGroup.DisplayOrder)
                .ThenBy(c => c.DisplayOrder)
                .ToList();

            return PartialView(categories.Select(c => Injector.Flat(new CategoryViewModel(), c)));
        }

        [ChildActionOnly]
        public PartialViewResult CategoryMenuWebPart(bool active)
        {
            var categoryMenuItems = new List<CategoryMenuItemViewModel>();
            var categoryGroups = _categoryService.Categories
                .GroupBy(c => c.CategoryGroup)
                .OrderBy(g => g.Key.DisplayOrder)
                .ToList();

            foreach (var group in categoryGroups)
            {
                var categoryGroupCaption = group.Key.Caption;
                CategoryMenuItemViewModel categoryMenuItem;
                // If just one category in a category group with the same caption
                if (group.Count() == 1 && categoryGroupCaption == group.Single().Caption)
                {
                    var c = group.Single();
                    categoryMenuItem = new CategoryMenuItemViewModel
                                       {
                                           Caption = categoryGroupCaption,
                                           Description = c.Description == "" ? c.Caption : c.Description,
                                           Url = Url.Route<ICategoryDetailsRoute>().Get(c.Id)
                                       };
                }
                // If more than one category in a category group
                else
                {
                    categoryMenuItem = new CategoryMenuItemViewModel
                                       {
                                           Caption = categoryGroupCaption,
                                           Description = categoryGroupCaption,
                                           Url = null,
                                           SubItems = group.OrderBy(c => c.DisplayOrder)
                                                           .Select(c => new CategoryMenuItemViewModel
                                                           {
                                                               Caption = c.Caption,
                                                               Description = c.Description == "" ? c.Caption : c.Description,
                                                               Url = Url.Route<ICategoryDetailsRoute>().Get(c.Id)
                                                           })
                                       };
                }
                categoryMenuItems.Add(categoryMenuItem);
            }

            return PartialView(new CategoryMenuWebPartViewModel { Active = active, MenuItems = categoryMenuItems });
        }
    }
}