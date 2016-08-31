using System;
using System.Web.Mvc;
using Edreamer.Framework.Context;
using Edreamer.Framework.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.Security;
using Edreamer.Framework.Settings;
using Rahnemun.CaptchaContracts;
using Rahnemun.Common;
using Rahnemun.ContactUsContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.ContactUs.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly ICaptchaService _captchaService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IContactUsService _contactUsService;
        private readonly ISettingsService _settingsService;
        private readonly IGuestService _guestService;

        public ContactUsController(ICaptchaService captchaService, IWorkContextAccessor workContextAccessor,
                                   IContactUsService contactUsService, ISettingsService settingsService, IGuestService guestService)
        {
            _captchaService = captchaService;
            _workContextAccessor = workContextAccessor;
            _contactUsService = contactUsService;
            _settingsService = settingsService;
            _guestService = guestService;
        }

        // GET: /contact
        [HttpGet]
        public ActionResult Contact()
        {
            var currentUser = _workContextAccessor.Context.CurrentUser();
            var isUserLoggedin = currentUser != null;
            var guest = isUserLoggedin ? null : _guestService.GetCurrentGuest();

            return View("ContactUs", new ContactUsEditViewModel
            {
                CaptchaId = Guid.NewGuid(),
                Subject = CustomerMessageSubject.Suggestion,
                IsUserLoggedin = isUserLoggedin,
                AddUrl = Url.Route<IContactUsRoute>().Get(),
                ResultMessage = "پیام شما با موفقیت دریافت و توسط کارشناسان رهنمون در اسرع وقت بررسی خواهد شد.",
                ResultMessageTitle = "ارتباط با ما",
                ContactTelNo = _settingsService.GetContactTelNo(),
                ContactTelTitle = _settingsService.GetContactTelTitle(),
                Email = guest?.Email,
                Name = guest?.Name
            });
        }

        #region Ajax Actions

        // POST: /contact
        [Ajax, HttpPost]
        public FormattedJsonResult Contact(ContactUsEditViewModel contactUsModel)
        {
            var currentUser = _workContextAccessor.Context.CurrentUser();
            var isUserLoggedin = currentUser != null;
            ValidateCustomerMessage(contactUsModel, isUserLoggedin);

            if (ModelState.IsValid)
            {
                if (isUserLoggedin)
                {
                    _contactUsService.PostCustomerMessage(contactUsModel.Subject, contactUsModel.Message, currentUser.Id, null);
                }
                else
                {
                    var guest = _guestService.SetCurrentGuest(contactUsModel.Email, contactUsModel.Name, Request.ServerVariables);
                    _contactUsService.PostCustomerMessage(contactUsModel.Subject, contactUsModel.Message, null, guest.Id);
                }
                return new FormattedJsonResult(new { Success = true, IsUserLoggedin = isUserLoggedin });
            }

            return new FormattedJsonResult(new { success = false, errors = ModelState.Errors(), isUserLoggedin }, false);
        }

        #endregion // Ajax Actions

        #region WebPart Actions

        [ChildActionOnly]
        public PartialViewResult FeedbackWebPart()
        {
            var currentUser = _workContextAccessor.Context.CurrentUser();
            var isUserLoggedin = currentUser != null;
            var guest = isUserLoggedin ? null : _guestService.GetCurrentGuest();

            var feedbackViewModel = new FeedbackEditViewModel
            {
                //CaptchaId = Guid.NewGuid(),
                IsUserLoggedin = isUserLoggedin,
                AddUrl = Url.Route<IContactUsRoute>().Get(),
                ResultMessage = "بازخورد شما با موفقیت ارسال شد! از اینکه نظر خود را با ما در میان گذاشتید متشکریم.",
                ResultMessageTitle = "سپاسگزاری",
                ContactTelNo = _settingsService.GetContactTelNo(),
                ContactTelTitle = _settingsService.GetContactTelTitle(),
                Email = guest?.Email
            };

            return PartialView("FeedbackWebPart", feedbackViewModel);
        }
        #endregion // WebPart Actions

        private void ValidateCustomerMessage(ContactUsEditViewModel customerMesageModel, bool isUserLoggedin)
        {
            if (isUserLoggedin || customerMesageModel.Subject == CustomerMessageSubject.Feedback)
            {
                ModelState["Name"].Errors.Clear();
                ModelState["Email"].Errors.Clear();
                ModelState["Captcha"].Errors.Clear();
            }
            else if (ModelState.IsValidField("Captcha") && !_captchaService.ValidateCaptcha(customerMesageModel.CaptchaId, customerMesageModel.Captcha))
                ModelState.AddModelError("Captcha", "کد امنیتی نادرست است.");
        }

    }
}
