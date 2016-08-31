using System;
using System.Net.Mail;
using System.Text;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Logging;
using Edreamer.Framework.Settings;
using MailChimp;
using MailChimp.Errors;
using MailChimp.Helper;
using MailChimp.Lists;
using Rahnemun.EmailContracts;

namespace Rahnemun.Email.Services
{
    public class EmailService: IEmailService
    {
        private readonly string _noreplyAddress;
        private readonly string _defaultSenderName;
        private readonly string _mailChimpApiKey;
        private readonly string _mailChimpListId;
        
        public EmailService(ISettingsService settingsService)
        {
            _noreplyAddress = settingsService.GetNoReplyEmail();
            _defaultSenderName = settingsService.GetDefaultSenderName();
            _mailChimpApiKey = settingsService.GetSetting<string>(new SettingEntryKey { Category = "RahnemunEmail", Name = "MailChimpApiKey" });
            _mailChimpListId = settingsService.GetSetting<string>(new SettingEntryKey { Category = "RahnemunEmail", Name = "MailChimpListId" });
        }

        public ILogger Logger { get; set; }

        public void SubscribeEmail(string emailAddress, string name, SubscriptionType type)
        {
            Throw.IfArgumentNullOrEmpty(emailAddress, "emailAddress");
            Throw.IfArgumentNullOrEmpty(name, "name");
            var mergeVars = new MergeVar {{"NAME", name}, {"TYPE", GetSubscriptionType(type)}};
            var mc = new MailChimpManager(_mailChimpApiKey);
            var email = new EmailParameter { Email = emailAddress };
            try
            {
                mc.Subscribe(_mailChimpListId, email, mergeVars, doubleOptIn: false, updateExisting: true);
            }
            catch (MailChimpAPIException ex)
            {
                Logger.Log(LogLevel.Error, ex, "MailChimp subscription failed for: {0} {1} {2}".FormatWith(emailAddress, name, type));
            }
        }

        private static string GetSubscriptionType(SubscriptionType type)
        {
            switch (type)
            {
                case SubscriptionType.Unknown: return "Unknown";
                case SubscriptionType.Consultant: return "Consultant";
                case SubscriptionType.Consultee: return "Consultee";
                default: throw new ArgumentOutOfRangeException("type");
            }
        }

        public void UnsubscribeEmail(string emailAddress)
        {
            Throw.IfArgumentNullOrEmpty(emailAddress, "emailAddress");
            var mc = new MailChimpManager(_mailChimpApiKey);
            var email = new EmailParameter { Email = emailAddress };
            try
            {
                mc.Unsubscribe(_mailChimpListId, email);
            }
            catch (MailChimpAPIException ex)
            {
                Logger.Log(LogLevel.Error, ex, "MailChimp unsubscription failed for: {0}".FormatWith(emailAddress));
            }
        }

        public void SendEmail(EmailModel email)
        {
            Throw.IfArgumentNull(email, "email");
            Throw.IfNullOrEmpty(email.ReceiverEmail).AnArgumentException("ReceiverEmail should be specified.", "email");
            var senderAddress = email.SenderEmail ?? _noreplyAddress;
            var senderName = email.SenderName ?? _defaultSenderName;
            var sender = new MailAddress(senderAddress, senderName, Encoding.UTF8);
            var receiver = new MailAddress(email.ReceiverEmail, email.ReceiverName, Encoding.UTF8);
            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(sender, receiver))
                {
                    message.Subject = email.Subject;
                    message.SubjectEncoding = Encoding.UTF8;
                    message.Body = email.Message;
                    message.BodyEncoding = Encoding.UTF8;
                    
                    message.IsBodyHtml = true;
                    client.Send(message);
                }
            }
        }
    }
}