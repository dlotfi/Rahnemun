using System.Web.Mvc;
using Edreamer.Framework.Mvc.Routes;
using Rahnemun.Common;
using Rahnemun.PaymentContracts;

namespace Rahnemun.Payment
{
    public class PaymentRoute : NamedRoute<PaymentRouteModel>, IPaymentRoute { }
    public class PaymentCompleteTransactionRoute : NamedRoute<PaymentCompleteTransactionRouteModel>, IPaymentCompleteTransactionRoute { }
   
    public class PaymentRouteRegistrar : IRouteRegistrar
    {
        public void RegisterRoutes(RouteRegistrarContext context)
        {
            context.MapRoute<PaymentRoute>("payment/{PaymentId}/{ProviderName}", new { Controller = "Payment", Action = "Payment" });
            context.MapRoute<PaymentCompleteTransactionRoute>("payment-complete/{ProviderName}", new { Controller = "Payment", Action = "PaymentCompleteTransaction" });
        }
    }
}