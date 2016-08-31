using System;
using System.Web.Mvc;
using Edreamer.Framework.Composition;

namespace Rahnemun.PaymentContracts
{
    [InterfaceExport]
    public interface IPaymentEventHandler
    {
        void OnSuccess(int paymentId, object handlerData, out Func<UrlHelper, string> route );
        void OnFailure(int paymentId, object handlerData, out Func<UrlHelper, string> route);
        void OnCleanUp(int paymentId, object handlerData);
        string HandlerId { get; }
    }
}
