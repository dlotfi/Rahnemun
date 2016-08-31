using System;
using System.Web.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.CategoryContracts;
using Rahnemun.Common;
using Rahnemun.PaymentContracts;
using Rahnemun.Session.Models;
using Rahnemun.SessionContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.Session.Services
{
    public class SessionPaymentEventHandler : IPaymentEventHandler
    {
        private readonly ISessionService _sessionService;
        private readonly ITracker _tracker;
        private readonly IConsultantService _consultantService;
        private readonly ICategoryService _categoryService;
        private readonly IPaymentService _paymentService;

        public const string Id = "SessionPaymentHandler";

        public SessionPaymentEventHandler(ISessionService sessionService, ITracker tracker,
            IConsultantService consultantService, ICategoryService categoryService, IPaymentService paymentService)
        {
            _sessionService = sessionService;
            _tracker = tracker;
            _consultantService = consultantService;
            _categoryService = categoryService;
            _paymentService = paymentService;
        }

        public void OnSuccess(int paymentId, object handlerData, out Func<UrlHelper, string> route)
        {
            var data = (SessionPaymentHandlerDataModel) handlerData;
            var session = _sessionService.CreateSession(data.ConsulteeId, data.ConsultantId, data.CategoryId, paymentId);
            route = url => url.Route<ISessionRoute>().Get(session.Id);
            _tracker.AddDestination("/session/payment-complete");

            var payment = _paymentService.GetPayment(paymentId);
            var consultant = _consultantService.GetConsultant(data.ConsultantId);
            var category = _categoryService.GetCategory(data.CategoryId);
            _tracker.AddTransaction(paymentId.ToString(), session.Id.ToString(), $"{consultant.LastName}، {consultant.FirstName}", $"{category.CategoryGroup.Caption} - {category.Caption}", payment.Amount);
        }

        public void OnFailure(int paymentId, object handlerData, out Func<UrlHelper, string> route)
        {
            var data = (SessionPaymentHandlerDataModel) handlerData;
            route = url => url.Route<IStartNewSessionRoute>().Get(new StartNewSessionRouteModel { ConsultantId = data.ConsultantId, CategoryId = data.CategoryId });
        }

        public void OnCleanUp(int paymentId, object handlerData)
        {
            throw new NotImplementedException();
        }

        public string HandlerId { get { return Id; } }
    }
}