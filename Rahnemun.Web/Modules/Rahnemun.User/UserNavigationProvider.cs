using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.CategoryContracts;
using Rahnemun.UserContracts;
using Rahnemun.UIContracts;

namespace Rahnemun.User
{
    public class UserNavigationProvider: INavigationProvider
    {
        private readonly ICategoryService _categoryService;
        private readonly IConsultantService _consultantService;
        private readonly IUserService _userService;

        public UserNavigationProvider(ICategoryService categoryService, IConsultantService consultantService, IUserService userService)
        {
            _categoryService = categoryService;
            _consultantService = consultantService;
            _userService = userService;
        }

        public IEnumerable<NavigationItemModel> GetNavigationItems()
        {
            yield return new NavigationItemModel
            {
                NavigationId = "JoinUs",
                Link = (url, requestUrl) => new Link { Url = url.Route<IConsultantJoinUsRoute>().Get(), Caption = "استخدام مشاور" },
                Priority = 200
            };
        }

        public bool TryGetSiteMapNodeInfo(SiteMapNodeModel node, out Func<UrlHelper, string> nodeRoute, out string nodeTitle, out SiteMapNodeModel parentNode)
        {
            switch (node.NavigationId)
            {
                case "CategoryDetails":
                    if (node.NavigationData == null || !node.NavigationData.ContainsKey("CategoryId") || !(node.NavigationData["CategoryId"] is int))
                    {
                        SetDefaultValues(out nodeRoute, out nodeTitle, out parentNode);
                        return false;
                    }
                    var categoryId = (int)node.NavigationData["CategoryId"];
                    var category = _categoryService.GetCategory(categoryId);
                    nodeRoute = url => url.Route<ICategoryDetailsRoute>().Get(categoryId);
                    nodeTitle = "گروه " + category.CategoryGroup.Caption + " - " + category.Caption;
                    parentNode = new SiteMapNodeModel("Category", null);
                    return true;
                case "ConsultantDisplay":
                    if (node.NavigationData == null || !node.NavigationData.ContainsKey("ConsultantId") || !(node.NavigationData["ConsultantId"] is int))
                    {
                        SetDefaultValues(out nodeRoute, out nodeTitle, out parentNode);
                        return false;
                    }
                    var consultantId = (int)node.NavigationData["ConsultantId"];
                    var consultant = _consultantService.GetConsultant(consultantId);
                    nodeTitle = _userService.GetUserFullName(consultant);
                    if (node.NavigationData.ContainsKey("CategoryId") && node.NavigationData["CategoryId"] is int)
                    {
                        var consultantCategoryId = (int)node.NavigationData["CategoryId"];
                        nodeRoute = url => url.Route<IConsultantDisplayRoute>().Get(new ConsultantIdModel { ConsultantId = consultantId, CategoryId = consultantCategoryId });
                        parentNode = new SiteMapNodeModel("CategoryDetails", new {CategoryId = consultantCategoryId });
                    }
                    else
                    {
                        nodeRoute = url => url.Route<IConsultantDisplayRoute>().Get(new ConsultantIdModel { ConsultantId = consultantId, CategoryId = null });
                        parentNode = new SiteMapNodeModel("Home", null);
                    }
                    return true;
                case "Dashboard":
                    nodeRoute = url => url.Route<IDashboardRoute>().Get();
                    nodeTitle = "صفحه کاربری";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "Login":
                    nodeRoute = url => url.Route<ILogInRoute>().Get(null);
                    nodeTitle = "ورود کاربر";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "PasswordReset":
                    nodeRoute = null;
                    nodeTitle = "تغییر کلمه عبور";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "VerifyConfirmEmail":
                    nodeRoute = null;
                    nodeTitle = "تایید ایمیل";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "ConfirmEmailRequest":
                    nodeRoute = url => url.Route<IConfirmEmailRequestRoute>().Get();
                    nodeTitle = "ارسال لینک تایید";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "PasswordResetRequest":
                    nodeRoute = url => url.Route<IPasswordResetRequestRoute>().Get();
                    nodeTitle = "بازنشانی کلمه عبور";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                //case "ConsultantPreliminaryRegister":
                //    nodeRoute = url => url.Route<IConsultantPreliminaryRegisterRoute>().Get();
                //    nodeTitle = "تشکیل پرونده";
                //    parentNode = new SiteMapNodeModel("Home", null);
                //    return true;
                case "JoinUs":
                    nodeRoute = url => url.Route<IConsultantJoinUsRoute>().Get();
                    nodeTitle = "استخدام در رهنمون";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "ConsultantFinalRegister":
                    nodeRoute = url => url.Route<IConsultantFinalRegisterRoute>().Get();
                    nodeTitle = "استخدام مشاور";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "ConsulteeRegisterOrLogin":
                    nodeRoute = null;
                    nodeTitle = "ورود یا تشکیل پرونده";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "UnauthorizedError":
                    nodeRoute = null;
                    nodeTitle = "خطای دسترسی";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                case "Unsubscribe":
                    nodeRoute = null;
                    nodeTitle = "لغو اشتراک";
                    parentNode = new SiteMapNodeModel("Home", null);
                    return true;
                default:
                    SetDefaultValues(out nodeRoute, out nodeTitle, out parentNode);
                    return false;
            }
        }

        private static void SetDefaultValues(out Func<UrlHelper, string> nodeRoute, out string nodeTitle, out SiteMapNodeModel parentNode)
        {
            nodeRoute = null;
            nodeTitle = null;
            parentNode = null;
        }
    }
}