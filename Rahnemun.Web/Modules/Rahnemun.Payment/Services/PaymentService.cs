using System;
using System.Linq;
using Edreamer.Framework.Helpers;
using Rahnemun.Domain;
using Rahnemun.PaymentContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.Payment.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRahnemunDataContext _dataContext;
       
        public PaymentService(IRahnemunDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<PaymentModel> Payments
        {
            get
            {
                return _dataContext.Payments.Select(p => new PaymentModel
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    HandlerId = p.HandlerId,
                    HandlerData = p.HandlerData,
                    Time = p.Time,
                    ProviderName = p.ProviderName,
                    RequestTime = p.RequestTime,
                    RequestId = p.RequestId,
                    ReferenceId = p.ReferenceId,
                    RequestResult = p.RequestResult,
                    VerificationTime = p.VerificationTime,
                    VerificationResult = p.VerificationResult,
                    User = new UserModel
                    {
                        Id = p.User.Id,
                        Timestamp = p.User.Timestamp,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        Gender = p.User.Gender,
                        EducationLevel = p.User.EducationLevel,
                        MaritalStatus = p.User.MaritalStatus,
                        CellphoneNo = p.User.CellphoneNo,
                        BirthDate = p.User.BirthDate,
                        SubscribedToNewsletter = p.User.SubscribedToNewsletter,
                        More = p.User.More,
                        RegisterDate = p.User.RegisterDate,
                        ProfilePictureId = p.User.ProfilePictureId
                    }
                });
            }
        }

        public PaymentModel GetPayment(int id)
        {
            return Payments.SingleOrDefault(p => p.Id == id);
        }

        public int AddPayment(int userId, string providerName, decimal price, string handlerId, string handlerData)
        {
            var paymentEntity = new Domain.Payment
            {
                UserId = userId,
                ProviderName = providerName,
                Amount = price,
                HandlerId = handlerId,
                HandlerData = handlerData,
                Time = DateTime.UtcNow
            };
        
            _dataContext.Payments.Add(paymentEntity);
            _dataContext.SaveChanges();

            return paymentEntity.Id;
        }

        public void SetPaymentRequest(int paymentId, string requestId, string requestResult)
        {
            var paymentEntity = _dataContext.Payments.Find(paymentId);
            Throw.If(paymentEntity == null)
                .AnArgumentException("No payment with id {0} exists.".FormatWith(paymentId), nameof(paymentId));
            paymentEntity.RequestResult = requestResult;
            paymentEntity.RequestId = requestId == String.Empty ? null : requestId;
            paymentEntity.RequestTime = DateTime.UtcNow;
            _dataContext.Payments.Update(paymentEntity, p => new { p.RequestId, p.RequestTime, p.RequestResult });
            _dataContext.SaveChanges();
        }

        public void SetVerificationResult(int paymentId, string referenceId, string verificationResult)
        {
            var paymentEntity = _dataContext.Payments.Find(paymentId);
            Throw.If(paymentEntity == null)
                .AnArgumentException("No payment with id {0} exists.".FormatWith(paymentId), nameof(paymentId));

            paymentEntity.VerificationResult = verificationResult;
            paymentEntity.ReferenceId = referenceId;
            paymentEntity.VerificationTime = DateTime.UtcNow;
            _dataContext.Payments.Update(paymentEntity, p => new { p.ReferenceId, p.VerificationTime, p.VerificationResult });
            _dataContext.SaveChanges();
        }

    }
}