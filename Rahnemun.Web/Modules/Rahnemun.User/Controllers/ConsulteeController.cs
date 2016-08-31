using System;
using System.Web.Mvc;
using Edreamer.Framework.Injection;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.Security;
using Edreamer.Framework.Security.Authentication;
using Edreamer.Framework.Settings;
using Edreamer.Framework.UI.Notification;
using Rahnemun.CaptchaContracts;
using Rahnemun.Common;
using Rahnemun.EmailContracts;
using Rahnemun.User.Models;
using Rahnemun.UserContracts;

namespace Rahnemun.User.Controllers
{
    public class ConsulteeController : Controller
    {
        private readonly IConsulteeService _consulteeService;
        private readonly INotifier _notifier;
        private readonly IUserService _userService;
        private readonly ISettingsService _settingsService;
        private readonly Edreamer.Framework.Security.Users.IUserService _frameworkUserService;
        private readonly ICaptchaService _captchaService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountService _accountService;
        private readonly ITracker _tracker;
        private readonly IConsultantService _consultantService;

        public ConsulteeController(IConsulteeService consulteeService, IConsultantService consultantService, INotifier notifier, 
            IUserService userService, ISettingsService settingsService, Edreamer.Framework.Security.Users.IUserService frameworkUserService,
            ICaptchaService captchaService, IAuthenticationService authenticationService, IAccountService accountService, ITracker tracker)
        {
            _consulteeService = consulteeService;
            _consultantService = consultantService;
            _notifier = notifier;
            _userService = userService;
            _settingsService = settingsService;
            _frameworkUserService = frameworkUserService;
            _captchaService = captchaService;
            _authenticationService = authenticationService;
            _accountService = accountService;
            _tracker = tracker;
        }

        // GET: /consultee/registerOrLogin?ReturnUrl=...
        [HttpGet]
        public ActionResult RegisterOrLogin(string returnUrl)
        {
            return View("ConsulteeRegisterOrLogin", new ConsulteeRegisterOrLoginModel
                {
                    RegisterViewModel = new ConsulteeRegisterViewModel { ReturnUrl = returnUrl, SubscribedToNewsletter = true , CaptchaId = Guid.NewGuid() },
                    LoginViewModel = new ConsulteeLoginViewModel { ReturnUrl = returnUrl },
                    IsRegisterMode = true
                });
        }

        // POST: /consultee/register
        [HttpPost]
        public ActionResult Register(ConsulteeRegisterViewModel registerViewModel)
        {
            ValidateConsulteeRegisterModel(registerViewModel);

            if (ModelState.IsValid)
            {
                var consultee = Injector.PlaneInject(new ConsulteeUpdateModel(), registerViewModel);
                _consulteeService.AddConsultee(consultee);
                
                // Send confirm email
                var fullName = _userService.GetUserFullName(Injector.PlaneInject(new UserModel(), registerViewModel));
                EmailCreatorFunc emailCreator = (u, nonce) =>
                    new EmailModel
                    {
                        ReceiverEmail = registerViewModel.Email,
                        ReceiverName = fullName,
                        Subject = "لطفا آدرس ایمیل خود را تایید نمایید",
                        Message = this.RenderView("../Account/ConfirmEmail", new EmailViewModel { Url = Url.Route<IConfirmEmailRoute>().Get(nonce), FullName = fullName })
                    };
                _accountService.SendChallengeEmail(registerViewModel.Email, emailCreator, false);

                // Login registered user
                var user = _frameworkUserService.ValidateUser(registerViewModel.Email, registerViewModel.Password, false);
                _authenticationService.SignIn(user, false);

                _notifier.Success("عضویت شما با موفقیت انجام شد. به منظور تایید ایمیل خود بر روی لینک تایید ایمیل که به آدرس ایمیل شما ارسال شده است کلیک نمایید.");
                _tracker.AddDestination("/consultee/register-complete");
                return this.SafeRedirect(registerViewModel.ReturnUrl, Url.Route<IDashboardRoute>().Get());
            }

            registerViewModel.Password = String.Empty;
            registerViewModel.ConfirmPassword = String.Empty;
            registerViewModel.Captcha = String.Empty;
            return View("ConsulteeRegisterOrLogin", new ConsulteeRegisterOrLoginModel
            {
                RegisterViewModel = registerViewModel,
                LoginViewModel = new ConsulteeLoginViewModel { ReturnUrl = registerViewModel.ReturnUrl },
                IsRegisterMode = true
            });
        }

        // POST: /consultee/login
        [HttpPost]
        public ActionResult Login(ConsulteeLoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _frameworkUserService.ValidateUser(loginViewModel.Email, loginViewModel.Password, false);
                if (user == null)
                {
                    ModelState.AddModelError("", "ایمیل یا کلمه عبور نامعتبر است.");
                }
                else if (user.IsPreliminaryRegisteredConsultant() || _consultantService.IsConsultant(user.Id))
                {
                    ModelState.AddModelError("", "مشاور عزیز! با حساب کاربری مشاور امکان دریافت خدمات مشاوره وجود ندارد. به منظور دریافت خدمات مشاوره با کاربر غیرمشاور وارد سایت شوید و در صورت نداشتن حساب کاربری غیر مشاور، حساب کاربری جدید ایجاد نمایید.");
                }
                else
                {
                    _authenticationService.SignIn(user, loginViewModel.RememberMe);
                    return this.SafeRedirect(loginViewModel.ReturnUrl, Url.Route<IDashboardRoute>().Get());
                }
            }

            loginViewModel.Password = String.Empty;
            return View("ConsulteeRegisterOrLogin", new ConsulteeRegisterOrLoginModel
            {
                RegisterViewModel = new ConsulteeRegisterViewModel { ReturnUrl = loginViewModel.ReturnUrl, CaptchaId = Guid.NewGuid() },
                LoginViewModel = loginViewModel,
                IsRegisterMode = false
            });
        }

        private void ValidateConsulteeRegisterModel(ConsulteeRegisterViewModel consulteeRegisterViewModel)
        {
            if (!ModelState.IsValid) return;

            if (!consulteeRegisterViewModel.Disclaimer)
                ModelState.AddModelError("", "تایید متن سلب مسئولیت الزامی است.");

            if (ModelState.IsValidField("Captcha") && !_captchaService.ValidateCaptcha(consulteeRegisterViewModel.CaptchaId, consulteeRegisterViewModel.Captcha))
            {
                ModelState.AddModelError("RegisterViewModel.Captcha", "کد امنیتی نادرست است.");
            }

            if (ModelState.IsValidField("Email") && !_frameworkUserService.VerifyEmailUnicity(consulteeRegisterViewModel.Email))
                ModelState.AddModelError("RegisterViewModel.Email", "ایمیل درج شده در رهنمون موجود است.");
            //More guide : اگر قبلاً با این ایمیل ثبت نام کرده اید برای ورود به سایت از لینک ورود به سایت در بالای صفحه استفاده کنید. در غیر اینصورت برای ثبت نام ایمیل دیگری را درج نمایید.

            if (ModelState.IsValidField("Password"))
                ModelState.AddPasswordValidationError(_settingsService.GetMembershipSettings(), consulteeRegisterViewModel.Password, "RegisterViewModel.Password");
        }

    }
}