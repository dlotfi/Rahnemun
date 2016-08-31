using System;
using System.Web.Mvc;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.Mvc.Security;
using Edreamer.Framework.Security;
using Edreamer.Framework.Security.Authentication;
using Edreamer.Framework.Settings;
using Edreamer.Framework.UI.Notification;
using Rahnemun.CaptchaContracts;
using Rahnemun.Common;
using Rahnemun.EmailContracts;
using Rahnemun.User.Models;
using Rahnemun.UserContracts;
using IFrameworkUserService = Edreamer.Framework.Security.Users.IUserService;

namespace Rahnemun.User.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IFrameworkUserService _frameworkUserService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICaptchaService _captchaService;
        private readonly IUserService _userService;
        private readonly INotifier _notifier;
        private readonly IEmailService _emailService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly ISettingsService _settingsService;
        private readonly IConsultantService _consultantService;

        public AccountController(IAccountService accountService, IFrameworkUserService frameworkUserService, IAuthenticationService authenticationService,
            ICaptchaService captchaService, IUserService userService, INotifier notifier, IEmailService emailService, IWorkContextAccessor workContextAccessor,
            ISettingsService settingsService, IConsultantService consultantService)
        {
            Throw.IfArgumentNull(accountService, "accountService");
            Throw.IfArgumentNull(frameworkUserService, "frameworkUserService");
            Throw.IfArgumentNull(authenticationService, "authenticationService");
            Throw.IfArgumentNull(captchaService, "captchaService");
            Throw.IfArgumentNull(userService, "userService");
            Throw.IfArgumentNull(notifier, "notifier");
            Throw.IfArgumentNull(emailService, "emailService");
            Throw.IfArgumentNull(workContextAccessor, "workContextAccessor");
            Throw.IfArgumentNull(settingsService, "settingsService");
            Throw.IfArgumentNull(consultantService, "consultantService");
            _accountService = accountService;
            _frameworkUserService = frameworkUserService;
            _authenticationService = authenticationService;
            _captchaService = captchaService;
            _userService = userService;
            _notifier = notifier;
            _emailService = emailService;
            _workContextAccessor = workContextAccessor;
            _settingsService = settingsService;
            _consultantService = consultantService;
        }

        // GET: /account/login?ReturnUrl=...
        [HttpGet]
        [AuthAction]
        public ActionResult Login(string returnUrl)
        {
            return View("Login", new LoginViewModel { ReturnUrl = returnUrl });
        }

        // POST: /account/login
        [HttpPost]
        [AuthAction]
        public ActionResult Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = _frameworkUserService.ValidateUser(loginModel.Email, loginModel.Password, false);
                if (user == null)
                {
                    ModelState.AddModelError("", "ایمیل یا کلمه عبور نامعتبر است.");
                }
                else if (/*user.IsPreliminaryRegisteredConsultant()*/ _consultantService.IsConsultant(user.Id) && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("Email", "ایمیل شما تایید نشده است.");
                    //More guide : لینک تایید به ایمیل شما ارسال شده است. به منظور ارسال مجدد لینک به ایمیل شما از طریق لینک فعال سازی ایمیل اقدام فرمایید.
                }
                else 
                {
                    _authenticationService.SignIn(user, loginModel.RememberMe);

                    var returnUrl = user.IsPreliminaryRegisteredConsultant()
                        ? Url.Route<IConsultantFinalRegisterRoute>().Get()
                        : Request.IsAjaxRequest() ? "" : loginModel.ReturnUrl;

                    if (Request.IsAjaxRequest())
                        return new FormattedJsonResult(new { Success = true, ReturnUrl = returnUrl });
                    return this.SafeRedirect(returnUrl, "/");
                }
            }
            if (Request.IsAjaxRequest())
                return new FormattedJsonResult(new { success = false, errors = ModelState.Errors() }, false);
            loginModel.Password = String.Empty;
            return View("Login", loginModel);
        }

        // GET/POST: /account/logout?ReturnUrl=...
        public ActionResult Logout(string returnUrl)
        {
            _authenticationService.SignOut();
            return this.SafeRedirect(returnUrl, "/");
        }


        // GET/POST: /account/unauthorized?Error=...
        [AuthAction]
        public ActionResult Unauthorized(string error)
        {
            Response.SuppressFormsAuthenticationRedirect = true;
            Response.TrySkipIisCustomErrors = true;
            var currentUser = _workContextAccessor.Context.CurrentUser();
            return View("UnauthorizedError", new UnauthorizedErrorViewModel
            {
                ErrorMessage = error,
                IsAuthenticated = currentUser != null,
                SupportEmail = _settingsService.GetSupportEmail()
            });
        }

        #region Email Confirmation

        // GET: /account/confirm-email?Nonce=...
        [HttpGet]
        [AuthAction]
        public ActionResult ConfirmEmail(string nonce)
        {
            string returnUrl;
            var userAccount = ValidateChallenge(nonce, false, out returnUrl);
            if (userAccount == null)
                return Redirect(returnUrl);

            // If user is consultee and already signed-in, confirm his email 
            if (_workContextAccessor.Context.CurrentUser()?.Id == userAccount.Id)
            {
                ConfirmEmailAndSubscribeToNewsletter(userAccount);
                _notifier.Success("ایمیل شما با موفقیت تایید شد.");
                return Redirect(Url.Route<IDashboardRoute>().Get());
            }

            // If user is consultant or is consultee but not signed-in he must verify email confirmation by providing his password.
            var fullName = userAccount.IsPreliminaryRegisteredConsultant()
                ? userAccount.GetPreliminaryRegisteredConsultantFullName()
                : _userService.GetUserFullName(_userService.GetUser(userAccount.Id));
            return View("VerifyConfirmEmail", new VerifyConfirmEmailViewModel { Nonce = nonce, FullName = fullName });
        }

        // POST: /account/confirm-email
        [HttpPost]
        [AuthAction]
        public ActionResult ConfirmEmail(VerifyConfirmEmailViewModel verifyEmailModel)
        {
            string returnUrl;
            var userAccount = ValidateChallenge(verifyEmailModel.Nonce, false, out returnUrl);
            if (userAccount == null)
                return Redirect(returnUrl);

            if (_frameworkUserService.ValidateUser(userAccount.Email, verifyEmailModel.Password, false) == null)
                ModelState.AddModelError("Password", "کلمه عبور نامعتبر است.");

            if (ModelState.IsValid)
            {
                ConfirmEmailAndSubscribeToNewsletter(userAccount);
                _authenticationService.SignIn(userAccount, false);
                _notifier.Success("ایمیل شما با موفقیت تایید شد.");
                returnUrl = userAccount.IsPreliminaryRegisteredConsultant()
                    ? Url.Route<IConsultantFinalRegisterRoute>().Get()
                    : Url.Route<IDashboardRoute>().Get();
                return Redirect(returnUrl);
            }

            verifyEmailModel.Password = String.Empty;
            return View("VerifyConfirmEmail", verifyEmailModel);
        }


        // GET: /account/request-confirm-email
        [HttpGet]
        [AuthAction]
        public ActionResult RequestConfirmEmail()
        {
            return View("SendEmail", new SendEmailViewModel
            {
                Purpose = SendEmailPurpose.ConfirmEmail,
                CaptchaId = Guid.NewGuid()
            });
        }

        // POST: /account/request-confirm-email
        [HttpPost]
        [AuthAction]
        public ActionResult RequestConfirmEmail(SendEmailViewModel sendEmailModel)
        {
            Edreamer.Framework.Security.User userAccount;
            ValidateSendEmailModel(sendEmailModel, false, out userAccount);
            if (ModelState.IsValid)
            {
                var user = _userService.GetUser(userAccount.Id);
                var fullName = user != null
                    ? _userService.GetUserFullName(user)
                    : userAccount.GetPreliminaryRegisteredConsultantFullName();
                var fullNameWithTitle = user != null
                    ? _userService.GetUserFullName(user, true)
                    : userAccount.GetPreliminaryRegisteredConsultantFullName();
                EmailCreatorFunc emailCreator = (u, nonce) =>
                    new EmailModel
                    {
                        ReceiverEmail = sendEmailModel.Email,
                        ReceiverName = fullName,
                        Subject = "لطفا آدرس ایمیل خود را تایید نمایید",
                        Message = this.RenderView("../Account/ConfirmEmail", new EmailViewModel { Url = Url.Route<IConfirmEmailRoute>().Get(nonce), FullName = fullNameWithTitle })
                    };
                _accountService.SendChallengeEmail(sendEmailModel.Email, emailCreator, false);
                _notifier.Success("لینک تایید ایمیل برای شما ارسال شد. لطفا به منظور تایید ایمیل خود بر روی لینک مربوطه کلیک نمایید.");
                return Redirect(Url.Route<IConfirmEmailRequestRoute>().Get());
            }
            sendEmailModel.Purpose = SendEmailPurpose.ConfirmEmail;
            sendEmailModel.Captcha = String.Empty;
            return View("SendEmail", sendEmailModel);
        }

        #endregion


        #region Password Reset

        // GET: /account/password-reset?Nonce=...
        [HttpGet]
        [AuthAction]
        public ActionResult PasswordReset(string nonce)
        {
            string returnUrl;
            var userAccount = ValidateChallenge(nonce, true, out returnUrl);
            if (userAccount == null)
                return Redirect(returnUrl);

            var fullName = userAccount.IsPreliminaryRegisteredConsultant()
                ? userAccount.GetPreliminaryRegisteredConsultantFullName()
                : _userService.GetUserFullName(_userService.GetUser(userAccount.Id));
            return View("PasswordReset", new PasswordResetViewModel { Nonce = nonce, FullName = fullName });
        }

        // POST: /account/password-reset
        [HttpPost]
        [AuthAction]
        public ActionResult PasswordReset(PasswordResetViewModel passwordResetModel)
        {
            string returnUrl;
            var userAccount = ValidateChallenge(passwordResetModel.Nonce, true, out returnUrl);
            if (userAccount == null)
                return Redirect(returnUrl);

            if (ModelState.IsValidField("Password"))
                ModelState.AddPasswordValidationError(_settingsService.GetMembershipSettings(), passwordResetModel.Password, "Password");

            if (ModelState.IsValid)
            {
                _frameworkUserService.SetPassword(userAccount.Id, passwordResetModel.Password);
                _authenticationService.SignIn(userAccount, false);
                _notifier.Success("کلمه عبور شما با موفقیت تغییر داده شد.");
                returnUrl = userAccount.IsPreliminaryRegisteredConsultant()
                    ? Url.Route<IConsultantFinalRegisterRoute>().Get()
                    : Url.Route<IDashboardRoute>().Get();
                return Redirect(returnUrl);
            }

            passwordResetModel.Password = String.Empty;
            passwordResetModel.ConfirmPassword = String.Empty;
            return View("PasswordReset", passwordResetModel);
        }

        // GET: /account/request-password-reset
        [HttpGet]
        [AuthAction]
        public ActionResult RequestPasswordReset()
        {
            return View("SendEmail", new SendEmailViewModel
            {
                Purpose = SendEmailPurpose.ResetPassword,
                CaptchaId = Guid.NewGuid()
            });
        }
        
        // POST: /account/request-password-reset
        [HttpPost]
        [AuthAction]
        public ActionResult RequestPasswordReset(SendEmailViewModel sendEmailModel)
        {
            Edreamer.Framework.Security.User userAccount;
            ValidateSendEmailModel(sendEmailModel, true, out userAccount);
            if (ModelState.IsValid)
            {
                var user = _userService.GetUser(userAccount.Id);
                var fullName = user != null
                    ? _userService.GetUserFullName(user)
                    : userAccount.GetPreliminaryRegisteredConsultantFullName();
                var fullNameWithTitle = user != null
                    ? _userService.GetUserFullName(user, true)
                    : userAccount.GetPreliminaryRegisteredConsultantFullName();
                EmailCreatorFunc emailCreator = (u, nonce) =>
                    new EmailModel
                    {
                        ReceiverEmail = sendEmailModel.Email,
                        ReceiverName = fullName,
                        Subject = "کلمه عبور خود را تغییر دهید",
                        Message = this.RenderView("../Account/PasswordResetEmail", new EmailViewModel { Url = Url.Route<IPasswordResetRoute>().Get(nonce), FullName = fullNameWithTitle })
                    };
                _accountService.SendChallengeEmail(sendEmailModel.Email, emailCreator, true);
                _notifier.Success("لینک تغییر کلمه عبور برای شما ارسال شد. لطفا به منظور تغییر کلمه عبور خود بر روی لینک مربوطه کلیک نمایید.");
                return Redirect(Url.Route<IPasswordResetRequestRoute>().Get());
            }
            sendEmailModel.Purpose = SendEmailPurpose.ResetPassword;
            sendEmailModel.Captcha = String.Empty;
            return View("SendEmail", sendEmailModel);
        } 

        #endregion


        private void ConfirmEmailAndSubscribeToNewsletter(Edreamer.Framework.Security.User userAccount)
        {
            userAccount.EmailConfirmed = true;
            _frameworkUserService.UpdateUser(userAccount);
            //if (userAccount.IsPreliminaryRegisteredConsultant()) // user is consultant
            //{
            //    if (((ConsultantPreliminaryData) userAccount.UserData).SubscribedToNewsletter)
            //    {
            //        var fullName = userAccount.GetPreliminaryRegisteredConsultantFullName();
            //        _emailService.SubscribeEmail(userAccount.Email, fullName, SubscriptionType.Consultant);
            //    }
            //}
            //else // user is consultee
            //{
            //    var user = _userService.GetUser(userAccount.Id);
            //    if (user.SubscribedToNewsletter)
            //    {
            //        var fullName = _userService.GetUserFullName(user);
            //        _emailService.SubscribeEmail(userAccount.Email, fullName, SubscriptionType.Consultee);
            //    }
            //}

            var user = _userService.GetUser(userAccount.Id);
            if (user.SubscribedToNewsletter)
            {
                var subscriptionType = _consultantService.IsConsultant(userAccount.Id)
                    ? SubscriptionType.Consultant
                    : SubscriptionType.Consultee;
                var fullName = _userService.GetUserFullName(user);
                _emailService.SubscribeEmail(userAccount.Email, fullName, subscriptionType);
            }
        }

        private Edreamer.Framework.Security.User ValidateChallenge(string nonce, bool forPasswordReset, out string returnUrl)
        {
            var userAccount = _accountService.ValidateChallenge(nonce);
            if (forPasswordReset)
            {
                if (userAccount == null)
                {
                    _notifier.Error("لینک تغییر کلمه عبور شما نامعتبر است یا به دلیل گذشت زمان زیاد اعتبار آن خاتمه یافته است. به منظور ارسال مجدد لینک تغییر کلمه عبور از طریق فرم زیر اقدام فرمایید.");
                    returnUrl = Url.Route<IPasswordResetRequestRoute>().Get();
                    return null;
                }
            }
            else
            {
                if (userAccount == null)
                {
                    _notifier.Error("لینک تایید ایمیل شما نامعتبر است یا به دلیل گذشت زمان زیاد اعتبار آن خاتمه یافته است. به منظور ارسال مجدد لینک تایید ایمیل از طریق فرم زیر اقدام فرمایید.");
                    returnUrl = Url.Route<IConfirmEmailRequestRoute>().Get();
                    return null;
                }
                if (userAccount.EmailConfirmed)
                {
                    _notifier.Information("ایمیل شما پیش‌تر تایید شده است.");
                    returnUrl = "/";
                    return null;
                }
            }
            returnUrl = null;
            return userAccount;
        }

        private void ValidateSendEmailModel(SendEmailViewModel sendEmailModel, bool forPasswordReset, out Edreamer.Framework.Security.User userAccount)
        {
            if (!ModelState.IsValid) { userAccount = null; return; }

            userAccount = _frameworkUserService.GetUser(sendEmailModel.Email);
            if (userAccount == null)
            {
                ModelState.AddModelError("Email", "ایمیل شما در رهنمون ثبت نشده است!");
            }
            else
            {
                if (!forPasswordReset && userAccount.EmailConfirmed)
                    ModelState.AddModelError("Email", "ایمیل شما پیش‌تر تایید شده است.");
                else if (forPasswordReset && !userAccount.EmailConfirmed)
                    ModelState.AddModelError("Email", "به منظور تغییر کلمه عبور لازم است ایمیل شما تایید شده باشد.");
            }

            if (ModelState.IsValidField("Captcha") && !_captchaService.ValidateCaptcha(sendEmailModel.CaptchaId, sendEmailModel.Captcha))
            {
                ModelState.AddModelError("Captcha", "کد امنیتی نادرست است.");
            }
        }
    }
}