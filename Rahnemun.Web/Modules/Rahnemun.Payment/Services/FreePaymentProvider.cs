using System;
using System.Collections.Generic;
using Edreamer.Framework.Helpers;
using Rahnemun.PaymentContracts;

namespace Rahnemun.Payment.Services
{
    public class FreePaymentProvider : IPaymentProvider
    {
        private readonly IPaymentService _paymentService;

        public FreePaymentProvider(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public string ProviderName { get { return "رایگان"; } }

        public bool RequestPayment(int paymentId, string callbackUrl, out PaymentRequestPlan paymentRequestPlan)
        {
            var payment = _paymentService.GetPayment(paymentId);
            Throw.IfNull(payment)
                .AnArgumentException("No Payment with the id {0} exists.".FormatWith(paymentId));
            // if price is not zero, free payment provider must throw an exception
            Throw.If(payment.Amount > 0)
                .A<InvalidOperationException>("FreePaymentProvider should be used for zero payments only.");

            //Update Payment Entity (paymentId, RequestId, RequestResult)
            _paymentService.SetPaymentRequest(paymentId, null, null);

            callbackUrl = AddQueryString(callbackUrl, "PaymentId=" + paymentId);
            paymentRequestPlan = new PaymentRequestPlan(false, callbackUrl, null);
            return true;
        }

        public bool VerifyPayment(IDictionary<string, string> paymentData, out int? paymentId)
        {
            paymentId = Int32.Parse(paymentData["PaymentId"]);

            Throw.IfNull(paymentId)
                .AnArgumentException("No Payment with the id {0} exists.".FormatWith(paymentId));

            _paymentService.SetVerificationResult((int)paymentId, null, null);
            return true;
        }

        private static string AddQueryString(string url, string param)
        {
            return url.Contains("?")
                ? url + "&" + param
                : url + "?" + param;
        }
    }
}