using Edreamer.Framework.Composition;

namespace Rahnemun.EmailContracts
{
    [InterfaceExport]
    public interface IEmailService
    {
        void SubscribeEmail(string emailAddress, string name, SubscriptionType type);
        void UnsubscribeEmail(string emailAddress);
        void SendEmail(EmailModel email);
    }
}