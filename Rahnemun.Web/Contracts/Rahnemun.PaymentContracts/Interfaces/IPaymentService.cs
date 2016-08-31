using System.Linq;
using System.Security.AccessControl;
using Edreamer.Framework.Composition;

namespace Rahnemun.PaymentContracts
{
    [InterfaceExport]
    public interface IPaymentService
    {
        IQueryable<PaymentModel> Payments { get; }
        PaymentModel GetPayment(int id);
        int AddPayment(int userId, string providerName, decimal price, string handlerId, string handlerData);
        void SetPaymentRequest(int paymentId, string requestId, string requestResult);
        void SetVerificationResult(int paymentId, string referenceId, string verificationResult);
    }
}
