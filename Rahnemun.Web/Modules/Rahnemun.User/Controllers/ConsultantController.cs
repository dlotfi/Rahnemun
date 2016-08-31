using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Injection;
using Edreamer.Framework.Media;
using Edreamer.Framework.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.Security;
using Edreamer.Framework.Settings;
using Edreamer.Framework.UI.Notification;
using Rahnemun.CaptchaContracts;
using Rahnemun.CategoryContracts;
using Rahnemun.Common;
using Rahnemun.ContactUsContracts;
using Rahnemun.EmailContracts;
using Rahnemun.User.Models;
using Rahnemun.UserContracts;

namespace Rahnemun.User.Controllers
{
    public class ConsultantController : Controller
    {
        private readonly IConsultantService _consultantService;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;
        private readonly INotifier _notifier;
        private readonly ISettingsService _settingsService;
        private readonly Edreamer.Framework.Security.Users.IUserService _frameworkUserService;
        private readonly ICaptchaService _captchaService;
        private readonly IMediaService _mediaService;
        private readonly IUserService _userService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IEmailService _emailService;
        private readonly ITracker _tracker;


        public ConsultantController(IConsultantService consultantService, ICategoryService categoryService,
            IAccountService accountService, INotifier notifier, ISettingsService settingsService, Edreamer.Framework.Security.Users.IUserService frameworkUserService,
            ICaptchaService captchaService, IMediaService mediaService, IUserService userService, IWorkContextAccessor workContextAccessor,
            IEmailService emailService, ITracker tracker)
        {
            _consultantService = consultantService;
            _categoryService = categoryService;
            _accountService = accountService;
            _notifier = notifier;
            _settingsService = settingsService;
            _frameworkUserService = frameworkUserService;
            _captchaService = captchaService;
            _mediaService = mediaService;
            _userService = userService;
            _workContextAccessor = workContextAccessor;
            _emailService = emailService;
            _tracker = tracker;
        }
        
        // GET: /consultants/{CategoryId}
        [HttpGet]
        public ActionResult Index(int categoryId)
        {
            var category = _categoryService.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null) return HttpNotFound();

            var consultants = _consultantService.Consultants(categoryId)
                .Where(c => c.Approved)
                .OrderBy(c => c.RegisterDate)
                .Select(c => new
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Gender = c.Gender,
                    EducationLevel = c.EducationLevel,
                    Title = c.Title,
                    ProfilePictureId = c.ProfilePictureId
                })
                .ToList();
            
            return View("ConsultantIndex", new ConsultantIndexViewModel
                {
                    CategoryId = categoryId,
                    CategoryCaption = category.Caption,
                    CategoryGroupCaption = category.CategoryGroup.Caption,
                    ConsultantsSummary = consultants.Select(c => new ConsultantSummaryViewModel
                    {
                        ConsultantProfileUrl = Url.Route<IConsultantDisplayRoute>().Get(new ConsultantIdModel { ConsultantId = c.Id, CategoryId = categoryId }),
                        FullName = _userService.GetUserFullName(Injector.PlaneInject(new UserModel(), c)),
                        Title = c.Title,
                        Gender = (Gender)c.Gender,
                        ProfilePictureId = c.ProfilePictureId
                    })
                });
        }

        // GET: /consultant/{ConsultantId}?CategoryId=...
        [HttpGet]
        public ActionResult Display(int consultantId, int? categoryId)
        {
            var consultant = _consultantService.GetConsultant(consultantId);
            if (consultant == null || !consultant.Approved) return HttpNotFound();

            var categoryCaption = categoryId != null
                ? _categoryService.GetCategory((int) categoryId)?.Caption
                : String.Join("، ", _consultantService.GetConsultantCategories(consultantId).Select(c => c.Caption));

            string consultantAge = null;
            if (consultant.BirthDate != null)
            {
                var age = DateTime.Today.Year - ((DateTime) consultant.BirthDate).Year;
                if (consultant.BirthDate > DateTime.Today.AddYears(-age)) age--;
                consultantAge = age + " سال";
            }

            return View("ConsultantDisplay", Injector.Flat(new ConsultantDisplayViewModel
                                                {
                                                    FullName = _userService.GetUserFullName(consultant),
                                                    Age = consultantAge,
                                                    CategoryId = categoryId,
                                                    CategoryCaption = categoryCaption
                                                }, consultant));
        }

        #region JoinUs

        // GET: /join
        [HttpGet]
        public ActionResult JoinUs()
        {
            var currentUser = _workContextAccessor.Context.CurrentUser();
            if (currentUser != null && currentUser.IsPreliminaryRegisteredConsultant())
                return Redirect(Url.Route<IConsultantFinalRegisterRoute>().Get());

            return View("JoinUs", new JoinUsViewModel
            {
                SubscribedToNewsletter = true,
                CategoryListItems = GetCategoriesSelectListItems(null),
                ContactTelNo = _settingsService.GetContactTelNo(),
                ContactTelTitle = _settingsService.GetContactTelTitle()
            });
        }

        // POST: /join
        [HttpPost]
        public ActionResult JoinUs(JoinUsViewModel viewModel)
        {
            var currentUser = _workContextAccessor.Context.CurrentUser();
            if (currentUser != null && currentUser.IsPreliminaryRegisteredConsultant())
                return Redirect(Url.Route<IConsultantFinalRegisterRoute>().Get());

            Media profilePictureMedia;
            ValidateJoinUsModel(viewModel, out profilePictureMedia);
            if (ModelState.IsValid)
            {
                // Preliminary register
                var registerModel = Injector.PlaneInject(new ConsultantPreliminaryRegisterModel(), viewModel);
                var id = _consultantService.PreliminaryRegisterConsultant(registerModel);

                // Final register
                if (profilePictureMedia != null)
                {
                    //Todo [02051218]: Resize profilePictureMedia to maximum size in setting
                    profilePictureMedia = _mediaService.AddMedia(profilePictureMedia);
                    viewModel.ProfilePictureId = profilePictureMedia.Id;
                }
                viewModel.Capacity = 100;
                viewModel.Id = id;
                var consultant = Injector.PlaneInject(new ConsultantUpdateModel(), viewModel);
                _consultantService.FinalRegisterConsultant(consultant);
                
                // Send confirm email
                var registeredUser = Injector.PlaneInject(new UserModel(), viewModel);
                EmailCreatorFunc emailCreator = (u, nonce) =>
                    new EmailModel
                    {
                        ReceiverEmail = viewModel.Email,
                        ReceiverName = _userService.GetUserFullName(registeredUser),
                        Subject = "لطفا آدرس ایمیل خود را تایید نمایید",
                        Message = this.RenderView("../Account/ConfirmEmail", new EmailViewModel { Url = Url.Route<IConfirmEmailRoute>().Get(nonce), FullName = _userService.GetUserFullName(registeredUser, true) })
                    };
                _accountService.SendChallengeEmail(viewModel.Email, emailCreator, false);
                
                _notifier.Success("اطلاعات شما با موفقیت دریافت شد. پرونده شما توسط کارشناسان رهنمون در اسرع وقت بررسی خواهد شد. به منظور فعال سازی حساب کاربری خود بر روی لینک تایید ایمیل که به آدرس ایمیل شما ارسال شده است کلیک نمایید.");
                _tracker.AddDestination("/consultant/register-complete");
                return Redirect(Url.Route<IConsultantJoinUsRoute>().Get());
            }

            viewModel.CategoryListItems = GetCategoriesSelectListItems(viewModel.CategoriesIds);
            viewModel.ContactTelNo = _settingsService.GetContactTelNo();
            viewModel.ContactTelTitle = _settingsService.GetContactTelTitle();
            viewModel.Password = String.Empty;
            viewModel.ConfirmPassword = String.Empty;
            return View("JoinUs", viewModel);
        }

        #endregion

        #region Preliminary Register

        //// GET: /consultant/register
        //[HttpGet]
        //public ActionResult PreliminaryRegister()
        //{
        //    var currentUser = _workContextAccessor.Context.CurrentUser();
        //    if (currentUser != null && currentUser.IsPreliminaryRegisteredConsultant())
        //        return Redirect(Url.Route<IConsultantFinalRegisterRoute>().Get());

        //    return View("ConsultantPreliminaryRegister", new ConsultantPreliminaryRegisterViewModel
        //    {
        //        SubscribedToNewsletter = true,
        //        CaptchaId = Guid.NewGuid()
        //    });
        //}

        //// POST: /consultant/register
        //[HttpPost]
        //public ActionResult PreliminaryRegister(ConsultantPreliminaryRegisterViewModel viewModel)
        //{
        //    var currentUser = _workContextAccessor.Context.CurrentUser();
        //    if (currentUser != null && currentUser.IsPreliminaryRegisteredConsultant())
        //        return Redirect(Url.Route<IConsultantFinalRegisterRoute>().Get());

        //    ValidatePreliminaryRegisterModel(viewModel);
        //    if (ModelState.IsValid)
        //    {
        //        var registerModel = Injector.PlaneInject(new ConsultantPreliminaryRegisterModel(), viewModel);
        //        _consultantService.PreliminaryRegisterConsultant(registerModel);

        //        // Send confirm email
        //        var fullName = viewModel.FirstName + " " + viewModel.LastName;
        //        EmailCreatorFunc emailCreator = (u, nonce) =>
        //            new EmailModel
        //            {
        //                ReceiverEmail = viewModel.Email,
        //                ReceiverName = fullName,
        //                Subject = "لطفا آدرس ایمیل خود را تایید نمایید",
        //                Message = this.RenderView("../Account/ConfirmEmail", new EmailViewModel { Url = Url.Route<IConfirmEmailRoute>().Get(nonce), FullName = fullName })
        //            };
        //        _accountService.SendChallengeEmail(viewModel.Email, emailCreator);

        //        _notifier.Success("به منظور فعال سازی حساب کاربری خود بر روی لینک تایید ایمیل که به آدرس ایمیل شما ارسال شده است کلیک نمایید.");
        //        _tracker.AddDestination("/consultant/preregister-complete");
        //        return Redirect(Url.Route<IConsultantPreliminaryRegisterRoute>().Get());
        //    }
        //    viewModel.Password = String.Empty;
        //    viewModel.ConfirmPassword = String.Empty;
        //    viewModel.Captcha = String.Empty;
        //    return View("ConsultantPreliminaryRegister", viewModel);
        //} 

        #endregion

        #region Final Register

        //// GET: /consultant/final-register
        //[HttpGet]
        //public ActionResult FinalRegister()
        //{
        //    var currentUser = _workContextAccessor.Context.CurrentUser();
        //    // Check to see if current user is a consultant and has not done final register yet
        //    if (currentUser == null)
        //        return new HttpUnauthorizedResult("به منظور تشکیل پرونده و تکمیل فرایند استخدام ورود شما به رهنمون الزامی است.");

        //    if (!currentUser.IsPreliminaryRegisteredConsultant())
        //        return new HttpUnauthorizedResult("تشکیل پرونده و تکمیل فرایند استخدام شما در رهنمون انجام شده است.");

        //    var currentUserData = (ConsultantPreliminaryData)currentUser.UserData;

        //    return View("ConsultantFinalRegister", new ConsultantEditViewModel
        //    {
        //        Id = currentUser.Id,
        //        FirstName = currentUserData.FirstName,
        //        LastName = currentUserData.LastName,
        //        SubscribedToNewsletter = currentUserData.SubscribedToNewsletter,
        //        CategoryListItems = GetCategoriesSelectListItems(null),
        //        Capacity = 10
        //    });
        //}

        //// POST: /consultant/final-register
        //[HttpPost]
        //public ActionResult FinalRegister(ConsultantEditViewModel consultantViewModel)
        //{
        //    var currentUser = _workContextAccessor.Context.CurrentUser();
        //    // Check to see if current user is a consultant, the same user as the edited user and has not done final register yet
        //    if (currentUser == null)
        //        return new HttpUnauthorizedResult("به منظور تشکیل پرونده و تکمیل فرایند استخدام ورود شما به رهنمون الزامی است.");

        //    if (consultantViewModel.Id != currentUser.Id)
        //        return new HttpUnauthorizedResult("شما اجازه تشکیل پرونده و تکمیل فرایند استخدام مشاور دیگر را ندارید.");

        //    if (!currentUser.IsPreliminaryRegisteredConsultant())
        //        return new HttpUnauthorizedResult("تشکیل پرونده و تکمیل فرایند استخدام شما در رهنمون انجام شده است.");

        //    Media profilePictureMedia;
        //    ValidateConsultant(consultantViewModel, out profilePictureMedia);

        //    if (ModelState.IsValid)
        //    {
        //        if (profilePictureMedia != null)
        //        {
        //            //Todo [02051218]: Resize profilePictureMedia to maximum size in setting
        //            profilePictureMedia = _mediaService.AddMedia(profilePictureMedia);
        //            consultantViewModel.ProfilePictureId = profilePictureMedia.Id;
        //        }

        //        var consultant = Injector.PlaneInject(new ConsultantUpdateModel(), consultantViewModel);
        //        _consultantService.FinalRegisterConsultant(consultant);

        //        // Send welcome email
        //        var user = Injector.PlaneInject(new UserModel(), consultantViewModel);
        //        var fullName = _userService.GetUserFullName(user);
        //        var fullNameWithTitle = _userService.GetUserFullName(user, true);
        //        var email = new EmailModel
        //        {
        //            ReceiverEmail = currentUser.Email,
        //            ReceiverName = fullName,
        //            Subject = "پرونده استخدام شما با موفقیت تکمیل شد.",
        //            Message = this.RenderView("../consultant/ConsultantWelcomeEmail", new WelcomeEmailViewModel { FullName = fullNameWithTitle })
        //        };
        //        _emailService.SendEmail(email);

        //        //Todo [10091756]: Due to following announcement, an email must be send after approving consultant information
        //        _notifier.Success("اطلاعات شما با موفقیت دریافت شد. پرونده شما توسط کارشناسان رهنمون در اسرع وقت بررسی خواهد شد و پس از دریافت ایمیل استخدام، شما جزو مشاوران رهنمون خواهید بود.");
        //        _tracker.AddDestination("/consultant/register-complete");
        //        return Redirect(Url.Route<IDashboardRoute>().Get());
        //    }

        //    consultantViewModel.CategoryListItems = GetCategoriesSelectListItems(consultantViewModel.CategoriesIds);
        //    return View("ConsultantFinalRegister", consultantViewModel);
        //}

        #endregion

        #region Edit

        // POST(Ajax): /consultant/edit
        [Ajax, HttpPost]
        public FormattedJsonResult Edit(ConsultantEditViewModel consultantViewModel)
        {
            var currentUser = _workContextAccessor.Context.CurrentUser();
            // Check to see if current user is a consultant, the same user as the edited user and has not done final register yet
            Throw.If(currentUser == null)
                .A<SecurityException>("Unauthorized to edit profile.", "شما مجوز لازم برای ویرایش پروفایل را ندارید.");
            Throw.If(consultantViewModel.Id != currentUser.Id)
                .A<SecurityException>("Unauthorized to edit other user's profile.", "شما مجوز لازم برای ویرایش پروفایل کاربر دیگر را ندارید.");
            Throw.If(currentUser.IsPreliminaryRegisteredConsultant())
                .A<SecurityException>("Unauthorized to edit profile before completing registration.", "شما پیش از تکمیل فرایند استخدام مجوز لازم برای ویرایش پروفایل را ندارید.");
            
            Media profilePictureMedia;
            ValidateConsultant(consultantViewModel, out profilePictureMedia);

            if (ModelState.IsValid)
            {
                if (profilePictureMedia != null)
                {
                    //Todo [02051218]: Resize profilePictureMedia to maximum size in setting
                    profilePictureMedia = _mediaService.AddMedia(profilePictureMedia);
                    consultantViewModel.ProfilePictureId = profilePictureMedia.Id;
                }

                var consultant = Injector.PlaneInject(new ConsultantUpdateModel(), consultantViewModel);
                _consultantService.UpdateConsultant(consultant, true);

                return new FormattedJsonResult(new { Success = true, Timestamp = consultant.Timestamp });
            }

            consultantViewModel.CategoryListItems = GetCategoriesSelectListItems(consultantViewModel.CategoriesIds);
            return new FormattedJsonResult(new { success = false, errors = ModelState.Errors() }, false);
        } 

        #endregion
        
        #region WebPart Actions

        [ChildActionOnly]
        public PartialViewResult UserCalltoActionWebPart(int consultantId, int? categoryId)
        {
            var consultant = _consultantService.GetConsultant(consultantId);
            Throw.If(consultant == null || !consultant.Approved)
                .AnArgumentException("No approved consultant with the id {0} exists.".FormatWith(consultantId), "consultantId");

            if (categoryId == null) return null;

            var category = _categoryService.GetCategory((int)categoryId);
            Throw.IfNull(category)
                .AnArgumentException("No category with the id {0} exists.".FormatWith(categoryId), "categoryId");
            var viewModel = new CategoryModel { Id = category.Id, Caption = category.Caption };
            return PartialView(viewModel);
        }

        [ChildActionOnly]
        public PartialViewResult ConsultantEditWebPart()
        {
            var currentUser = _workContextAccessor.Context.CurrentUser();
            bool hasNewData;
            var consultant = _consultantService.GetConsultant(currentUser.Id, out hasNewData);
            var consultantCategories = _consultantService.GetConsultantCategories(currentUser.Id, true).Select(c => c.Id);
            return PartialView(Injector.PlaneInject(new ConsultantEditViewModel
                                    {
                                        UpdatePending = hasNewData,
                                        CategoryListItems = GetCategoriesSelectListItems(consultantCategories),
                                    }, consultant));
        }

        #endregion

        private IEnumerable<SelectListItem> GetCategoriesSelectListItems(IEnumerable<int> selectedCategoriesIds)
        {
            selectedCategoriesIds = CollectionHelpers.EmptyIfNull(selectedCategoriesIds);
            var categoryGroups = _categoryService.Categories
                .GroupBy(c => c.CategoryGroup)
                .OrderBy(g => g.Key.DisplayOrder)
                .ToList();
            foreach (var categoryGroup in categoryGroups)
            {
                var group = new SelectListGroup { Name = categoryGroup.Key.Caption };
                foreach (var category in categoryGroup.OrderBy(c => c.DisplayOrder))
                    yield return new SelectListItem
                    {
                        Group = group,
                        Text = category.Caption,
                        Value = category.Id.ToString(),
                        Selected = selectedCategoriesIds.Contains(category.Id)
                    };
            }
        }

        #region Validation Methods

        private void ValidateJoinUsModel(JoinUsViewModel viewModel, out Media consultantProfilePictureMedia)
        {
            if (!viewModel.Disclaimer)
                ModelState.AddModelError("Disclaimer", "تایید شرایط و قوانین رهنمون الزامی است.");

            if (ModelState.IsValidField("Email") && !_frameworkUserService.VerifyEmailUnicity(viewModel.Email))
                ModelState.AddModelError("Email", "ایمیل درج شده در رهنمون موجود است.");

            if (ModelState.IsValidField("Password"))
                ModelState.AddPasswordValidationError(_settingsService.GetMembershipSettings(), viewModel.Password, "Password");

            ValidateConsultant(viewModel, out consultantProfilePictureMedia);
        }

        //private void ValidatePreliminaryRegisterModel(ConsultantPreliminaryRegisterViewModel viewModel)
        //{
        //    if (!viewModel.Disclaimer)
        //        ModelState.AddModelError("", "تایید متن سلب مسئولیت الزامی است.");

        //    if (ModelState.IsValidField("Captcha") && !_captchaService.ValidateCaptcha(viewModel.CaptchaId, viewModel.Captcha))
        //    {
        //        ModelState.AddModelError("Captcha", "کد امنیتی نادرست است.");
        //    }

        //    if (ModelState.IsValidField("Email") && !_frameworkUserService.VerifyEmailUnicity(viewModel.Email))
        //        ModelState.AddModelError("Email", "ایمیل درج شده در رهنمون موجود است.");
        //    //More guide : اگر قبلاً با این ایمیل ثبت نام کرده اید برای ورود به سایت از لینک ورود به سایت در بالای صفحه استفاده کنید. در غیر اینصورت برای ثبت نام ایمیل دیگری را درج نمایید.

        //    if (ModelState.IsValidField("Password"))
        //        ModelState.AddPasswordValidationError(_settingsService.GetMembershipSettings(), viewModel.Password, "Password");
        //}

        private void ValidateConsultant(ConsultantEditViewModel consultantViewModel, out Media consultantProfilePictureMedia)
        {
            consultantProfilePictureMedia = null;
            if (consultantViewModel.ProfilePicture != null)
            {
                consultantProfilePictureMedia = _mediaService.CreateMedia(consultantViewModel.ProfilePicture.InputStream, consultantViewModel.ProfilePicture.FileName);

                if (!_mediaService.IsMediaAcceptable(consultantProfilePictureMedia) || consultantProfilePictureMedia.TypeGroup != "image")
                    ModelState.AddModelError("ProfilePicture", "تصویر آپلود شده غیر قابل قبول است.");
            }
            //else if (consultantViewModel.ProfilePictureId == null)
            //{
            //    ModelState.AddModelError("ProfilePicture", "درج تصویر پروفایل الزامی است.");
            //}

            //Todo-Low [25071240]: Validate BankCardNumber with BankName

            if (consultantViewModel.BirthDate != null)
            {
                var age = DateTime.Today.Year - ((DateTime) consultantViewModel.BirthDate).Year;
                if (consultantViewModel.BirthDate > DateTime.Today.AddYears(-age)) age--;
                if (age < 20)
                    ModelState.AddModelError("BirthDate", "تنها اشخاص بالای 20 سال می‌توانند به عنوان مشاور استخدام شوند.");
            }

            if (consultantViewModel.CategoriesIds == null || !consultantViewModel.CategoriesIds.Any())
            {
                ModelState["CategoriesIds"].Errors.Clear();
                ModelState.AddModelError("CategoriesIds", "انتخاب حداقل یک گروه مشاوره الزامی است.");
            }
        } 

        #endregion

    }
}