using System;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using Rahnemun.EmailContracts;
using Rahnemun.UserContracts;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Security.Encryption;
using Edreamer.Framework.Settings;
using IUserService = Edreamer.Framework.Security.Users.IUserService;

namespace Rahnemun.User.Services
{
    public class AccountService: IAccountService
    {
        private readonly IUserService _frameworkUserService;
        private readonly IEncryptionService _encryptionService;
        private readonly ISettingsService _settingsService;
        private readonly IEmailService _emailService;

        public AccountService(IUserService frameworkUserService, IEncryptionService encryptionService,
            ISettingsService settingsService, IEmailService emailService)
        {
            Throw.IfArgumentNull(frameworkUserService, "frameworkUserService");
            Throw.IfArgumentNull(encryptionService, "encryptionService");
            Throw.IfArgumentNull(settingsService, "settingsService");
            Throw.IfArgumentNull(emailService, "emailService");
            _frameworkUserService = frameworkUserService;
            _encryptionService = encryptionService;
            _settingsService = settingsService;
            _emailService = emailService;
        }

        public bool SendChallengeEmail(string usernameOrEmail, EmailCreatorFunc emailCreator, bool forPasswordReset)
        {
            Throw.IfArgumentNullOrEmpty(usernameOrEmail, "usernameOrEmail");
            Throw.IfArgumentNull(emailCreator, "emailCreator");
            var user = _frameworkUserService.GetUser(usernameOrEmail);
            if (user == null || user.Disabled || (forPasswordReset && !user.EmailConfirmed) || (forPasswordReset && !user.Approved))
                return false;
            var delay = forPasswordReset
                ? _settingsService.GetSetting<TimeSpan>(new SettingEntryKey { Category = "RahnemunUser", Name = "DelayToResetPassword" })
                : _settingsService.GetSetting<TimeSpan>(new SettingEntryKey { Category = "RahnemunUser", Name = "DelayToConfirmEmail" });
                 
            var nonce = CreateNonce(user, delay);
            var email = emailCreator(user, nonce);
            _emailService.SendEmail(email);
            return true;
        }

        public Edreamer.Framework.Security.User ValidateChallenge(string nonce)
        {
            int userId;
            DateTime validateByUtc;

            if (!DecryptNonce(nonce, out userId, out validateByUtc) || DateTime.UtcNow > validateByUtc)
                return null;

            return _frameworkUserService.GetUser(userId);
        }

        private string CreateNonce(Edreamer.Framework.Security.User user, TimeSpan delay)
        {
            var challengeToken = new XElement("n", 
                new XAttribute("uid", user.Id.ToString(CultureInfo.InvariantCulture)), 
                new XAttribute("utc", DateTime.UtcNow.ToUniversalTime().Add(delay).ToString(CultureInfo.InvariantCulture))).ToString();
            var data = Encoding.UTF8.GetBytes(challengeToken);
            return Convert.ToBase64String(_encryptionService.Encode(data));
        }

        private bool DecryptNonce(string nonce, out int userId, out DateTime validateByUtc)
        {
            userId = 0;
            validateByUtc = DateTime.UtcNow;

            try
            {
                var data = _encryptionService.Decode(Convert.FromBase64String(nonce));
                var xml = Encoding.UTF8.GetString(data);
                var element = XElement.Parse(xml);
                userId = Int32.Parse(element.Attribute("uid").Value, CultureInfo.InvariantCulture);
                validateByUtc = DateTime.Parse(element.Attribute("utc").Value, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}