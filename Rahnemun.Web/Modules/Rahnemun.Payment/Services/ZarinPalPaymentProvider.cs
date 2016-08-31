using System;
using System.Collections.Generic;
using System.Linq;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Logging;
using Rahnemun.PaymentContracts;
using Rahnemun.Payment.ZarinPal;

namespace Rahnemun.Payment.Services
{
    public class ZarinPalPaymentProvider : IPaymentProvider
    {
        private readonly IPaymentService _paymentService;

        private const string MerchantId = "SENSITIVE_INFO_HERE";
        //Todo [05081849]: check for zarinpal response time
        private const int Timeout = 40000; //40 Seconds 

        public ZarinPalPaymentProvider(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public ILogger Logger { get; set; }

        public string ProviderName { get { return "زرین پال"; } }

        public bool RequestPayment(int paymentId, string callbackUrl, out PaymentRequestPlan paymentRequestPlan)
        {
            var payment = _paymentService.GetPayment(paymentId);
            Throw.IfNull(payment)
                .AnArgumentException("No Payment with the id {0} exists.".FormatWith(paymentId));

            paymentRequestPlan = null;
            string authority = null;
            int status;

            System.Net.ServicePointManager.Expect100Continue = false;
            var zp = new PaymentGatewayImplementationService { Timeout = Timeout };
            var description = "هزینه جلسه مشاوره (شماره پرداخت  {0})".FormatWith(paymentId);

            try
            {
                status = zp.PaymentRequest(MerchantId, payment.Amount.ToString("G0"), description, null, null, callbackUrl, out authority);
                
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Problem occured during ZarinPalPaymentProvider.PaymentRequest.");
                //Update Payment Entity (PaymentId, RequestId, RequestResult)
                _paymentService.SetPaymentRequest(paymentId, authority, "ERROR");
                return false;
            }

            //Update Payment Entity (PaymentId, RequestId, RequestResult)
            _paymentService.SetPaymentRequest(paymentId, authority, status.ToString());

            if (status != 100) return false;
            
            //https://www.zarinpal.com/pg/StartPay/$Authority/MobileGate -> Redirect to MobileGate
            //https://www.zarinpal.com/pg/StartPay/$Authority            -> Redirect to WebGate
            //https://www.zarinpal.com/pg/StartPay/$Authority/ZarinGate  -> Redirect to ZarinGate
            //ZarinGate will directly redirect to bank payment page but WebGate will redirect to zarinpal payment page first.
            paymentRequestPlan = new PaymentRequestPlan(false, "https://www.zarinpal.com/pg/StartPay/{0}/ZarinGate".FormatWith(authority), null);
            return true;
        }

        public bool VerifyPayment(IDictionary<string, string> paymentData, out int? paymentId)
        {
            paymentId = null;
          
            Throw.If(String.IsNullOrEmpty(paymentData["Status"]) || String.IsNullOrEmpty(paymentData["Authority"]))
                .AnArgumentException("Invalid data was sent to ZarinPalPaymentProvider.VerifyPayment.");

            var authority = paymentData["Authority"];
            var payment = _paymentService.Payments.FirstOrDefault(p => p.RequestId == authority);
            if (payment == null) return false;

            paymentId = payment.Id;
            //If the payment has been already verified. 
            //User might go back and try completing the payment, so previous "NOK" should not stop verification.
            if (!String.IsNullOrEmpty(payment.VerificationResult) && payment.VerificationResult != "NOK") return false;

            if (paymentData["Status"].Equals("NOK"))
            {
                _paymentService.SetVerificationResult((int)paymentId, null, "NOK");
                return false;
            }

            var amount = (int)payment.Amount;
            long refId;
            int status;

            System.Net.ServicePointManager.Expect100Continue = false;
            var zp = new PaymentGatewayImplementationService { Timeout = Timeout };
            
            try
            {
                status = zp.PaymentVerification(MerchantId, authority, amount, out refId);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Problem occured during ZarinPalPaymentProvider.VerifyPayment.");
                _paymentService.SetVerificationResult((int)paymentId, null, "ERROR");
                return false;
            }

            _paymentService.SetVerificationResult((int)paymentId, status != 100 ? null : refId.ToString(), status.ToString());
            return true;
        }
    }
}