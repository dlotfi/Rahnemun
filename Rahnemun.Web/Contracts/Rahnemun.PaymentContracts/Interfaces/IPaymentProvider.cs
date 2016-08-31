using System.Collections.Generic;
using Edreamer.Framework.Composition;

namespace Rahnemun.PaymentContracts
{
    [InterfaceExport]
    public interface IPaymentProvider
    {
        string ProviderName { get; }
        bool RequestPayment(int paymentId, string callbackUrl, out PaymentRequestPlan paymentRequestPlan);
        bool VerifyPayment(IDictionary<string, string> paymentData, out int? paymentId);
    }
}
