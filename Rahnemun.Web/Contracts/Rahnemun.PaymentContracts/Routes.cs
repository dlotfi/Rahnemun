using Edreamer.Framework.Mvc.Routes;
using Rahnemun.Common;

namespace Rahnemun.PaymentContracts
{
    public interface IPaymentRoute : INamedRoute<PaymentRouteModel> { }
    public interface IPaymentCompleteTransactionRoute : INamedRoute<PaymentCompleteTransactionRouteModel> { }
}