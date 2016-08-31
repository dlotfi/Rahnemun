using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.UI.Notification;
using Rahnemun.PaymentContracts;

namespace Rahnemun.Payment.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly INotifier _notifier;
        private readonly IEnumerable<IPaymentEventHandler> _paymentEventHandlers;
        private readonly IEnumerable<IPaymentProvider> _paymentProviders;

        public PaymentController(IPaymentService paymentService, IEnumerable<IPaymentEventHandler> paymentEventHandlers,
                                 INotifier notifier, IEnumerable<IPaymentProvider> paymentProviders)
        {
            _paymentService = paymentService;
            _notifier = notifier;
            _paymentProviders = paymentProviders;
            _paymentEventHandlers = CollectionHelpers.EmptyIfNull(paymentEventHandlers);
        }


        // GET: /payment/{Id}
        // 1. Find provider by its providerId
        // 2. Call RequestPayment method on provider
        // 3. Evaluate return value of RequestPayment which is an object of PaymentRequestPlan
        //     * If plan.Post is false redirect to plan.Url
        //     * otherwise return a ViewResult containing a form which is populated with plan.Data items as hidden inputs (IDictionary<string, string>) 
        //        and will be posted by JavaScript
        [HttpGet]
        public ActionResult Payment(int paymentId, string providerName)
        {
            var paymentProvider = _paymentProviders.Single(h => h.ProviderName == providerName);
            var callbackUrl = Url.ConvertToAbsoluteUrl(Url.Route<IPaymentCompleteTransactionRoute>().Get(new PaymentCompleteTransactionRouteModel { ProviderName = providerName }));
            PaymentRequestPlan paymentRequestPlan;

            if (!paymentProvider.RequestPayment(paymentId, callbackUrl, out paymentRequestPlan))
            {
                //use failure action to handle errors
                var payment = _paymentService.GetPayment(paymentId);
                var handler = _paymentEventHandlers.Single(h => h.HandlerId == payment.HandlerId);
                Func<UrlHelper, string> route;
                handler.OnFailure(payment.Id, SerializationHelpers.Deserialize(payment.HandlerData), out route);
                _notifier.Error("با عرض پوزش، در حال حاضر ارتباط با درگاه پرداخت {0} قطع است. لطفا دوباره تلاش نمایید.".FormatWith(providerName));
                return Redirect(route(Url));
            }

            if (paymentRequestPlan.Post)
            {
                //Todo [05081826]: return a ViewResult containing a form
            }

            return Redirect(paymentRequestPlan.Url);
        }

        // GET: /payment-complete/{ProviderName}
        // 1. Get all http request data: Query string & Form data --> allRequestData
        // 2. Call VerifyPayment and send all http request data as its IDictionary<string, string> parameter
        // 3. Check VerifyPayment return value which is bool or an enum indicating payment status
        // 4. If payment was succeed, get payment Id by out parameter of VerifyPayment
        // 5. Execute the rest of code:
        [HttpGet]
        public ActionResult PaymentCompleteTransaction(string providerName)
        {
            var formData = Request.Form.Cast<string>().ToDictionary(p => p, p => Request.Form[p]);
            var queryStringData = Request.QueryString.Cast<string>().ToDictionary(p => p, p => Request.QueryString[p]);
            var allRequestData = CollectionHelpers.MergeDictionaries(formData, queryStringData);

            var paymentProvider = _paymentProviders.Single(h => h.ProviderName == providerName);
            int? paymentId;
            var success = paymentProvider.VerifyPayment(allRequestData, out paymentId);

            Throw.IfNull(paymentId)
                .A<InvalidOperationException>("Invalid data was sent to PaymentCompleteTransaction by payment gateway.");
            
            var payment = _paymentService.GetPayment((int) paymentId);

            var handler = _paymentEventHandlers.Single(h => h.HandlerId == payment.HandlerId);
            Func<UrlHelper, string> route;
            if (!success)
            {
                handler.OnFailure(payment.Id, SerializationHelpers.Deserialize(payment.HandlerData), out route);
                _notifier.Error("متاسفانه پرداخت شما با خطا مواجه شده است. برای انجام عملیات پرداخت و شروع جلسه دوباره تلاش نمایید.");
            }
            else
            {
                handler.OnSuccess(payment.Id, SerializationHelpers.Deserialize(payment.HandlerData), out route);
                if(payment.Amount > 0) //for free pay we dont need to show a message
                    _notifier.Success("پرداخت شما با کد پیگیری {0} با موفقیت انجام شد.".FormatWith(payment.ReferenceId));
            }
            return Redirect(route(Url));
        }
    }
}