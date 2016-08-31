using System;
using System.Linq;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Settings;
using Rahnemun.CategoryContracts;
using Rahnemun.Common;
using Rahnemun.Domain;
using Rahnemun.SessionContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.Session.Services
{
    public class SessionService : ISessionService
    {
        private readonly IRahnemunDataContext _dataContext;
        private readonly ISettingsService _settingsService;

        public SessionService(IRahnemunDataContext dataContext, ISettingsService settingsService)
        {
            _dataContext = dataContext;
            _settingsService = settingsService;
        }

        public IQueryable<SessionModel> Sessions 
        {
            get
            {
                return _dataContext.Sessions.Select(s => new SessionModel
                {
                    Id = s.Id,
                    StartTime = s.StartTime,
                    StopTime = s.StopTime,
                    StopType = s.StopType,
                    Rating = s.Rating,
                    Consultee = new UserModel
                    {
                        Id = s.ConsulteeId,
                        FirstName = s.Consultee.FirstName,
                        LastName = s.Consultee.LastName,
                        Gender = s.Consultee.Gender,
                        EducationLevel = s.Consultee.EducationLevel,
                        MaritalStatus = s.Consultee.MaritalStatus,
                        CellphoneNo = s.Consultee.CellphoneNo,
                        BirthDate = s.Consultee.BirthDate,
                        SubscribedToNewsletter = s.Consultee.SubscribedToNewsletter,
                        More = s.Consultee.More,
                        RegisterDate = s.Consultee.RegisterDate,
                        ProfilePictureId = s.Consultee.ProfilePictureId
                    },
                    Consultant = new UserModel
                    {
                        Id = s.ConsultantId,
                        FirstName = s.Consultant.FirstName,
                        LastName = s.Consultant.LastName,
                        Gender = s.Consultant.Gender,
                        EducationLevel = s.Consultant.EducationLevel,
                        MaritalStatus = s.Consultant.MaritalStatus,
                        CellphoneNo = s.Consultant.CellphoneNo,
                        BirthDate = s.Consultant.BirthDate,
                        SubscribedToNewsletter = s.Consultant.SubscribedToNewsletter,
                        More = s.Consultant.More,
                        RegisterDate = s.Consultant.RegisterDate,
                        ProfilePictureId = s.Consultant.ProfilePictureId
                    },
                    Category = new CategoryModel
                    {
                        Id = s.CategoryId,
                        Caption = s.Category.Caption,
                        Description = s.Category.Description,
                        Terms = s.Category.Terms,
                        DisplayOrder = s.Category.DisplayOrder,
                        CategoryGroup = new CategoryGroupModel { Id = s.Category.CategoryGroupId }
                    },
                    //Payment = new PaymentModel
                    //{
                    //    Id = s.Payment.Id,
                    //    Amount = s.Payment.Amount,
                    //    HandlerId = s.Payment.HandlerId,
                    //    HandlerData = s.Payment.HandlerData,
                    //    Time = s.Payment.Time,
                    //    ProviderName = s.Payment.ProviderName,
                    //    RequestTime = s.Payment.RequestTime,
                    //    RequestId = s.Payment.RequestId,
                    //    ReferenceId = s.Payment.ReferenceId,
                    //    RequestResult = s.Payment.RequestResult,
                    //    VerificationTime = s.Payment.VerificationTime,
                    //    VerificationResult = s.Payment.VerificationResult,
                    //    User = new UserModel { Id = s.Payment.UserId }
                    //}
                });
            }
        }
        public SessionModel GetSession(int id)
        {
            return Sessions.SingleOrDefault(s => s.Id == id);
        }

        public SessionModel CreateSession(int consulteeId, int consultantId, int categoryId, int paymentId)
        {
            var sessionEntity = new Domain.Session
                                {
                                    ConsulteeId = consulteeId,
                                    ConsultantId = consultantId,
                                    CategoryId = categoryId,
                                    PaymentId = paymentId,
                                    StartTime = DateTime.UtcNow
                                };
            _dataContext.Sessions.Add(sessionEntity);
            _dataContext.SaveChanges();
            return GetSession(sessionEntity.Id);
        }

        public DateTime StopSession(int id, SessionStopType sessionStopType)
        {
            var sessionEntity = _dataContext.Sessions.Find(id);
            Throw.If(sessionEntity == null)
                .AnArgumentException("No session with id {0} exists.".FormatWith(id), nameof(id));
            sessionEntity.StopTime = DateTime.UtcNow;
            sessionEntity.StopType = sessionStopType;
            _dataContext.Sessions.Update(sessionEntity, s => new { s.StopTime, s.StopType });
            _dataContext.SaveChanges();
            return (DateTime)sessionEntity.StopTime;
        }

        public void DeleteSession(int id)
        {
            _dataContext.Sessions.Remove(null, id);
            _dataContext.SaveChanges();
        }

        public void RateSession(int id, byte ratingValue)
        {
            var sessionEntity = _dataContext.Sessions.Find(id);
            Throw.If(sessionEntity == null)
                .AnArgumentException("No session with id {0} exists.".FormatWith(id), nameof(id));
            sessionEntity.Rating = ratingValue;
            _dataContext.Sessions.Update(sessionEntity, s => new { s.Rating });
            _dataContext.SaveChanges();
        }

        public int GetSessionElapsedTime(int id)
        {
            var lastConsultantMessageTime = _dataContext.Messages
                .Where(m => m.SessionId == id && !m.ByConsultee)
                .Max(m => (DateTime?)m.SentTime) ?? DateTime.MinValue;

            double consulteeCharCount = _dataContext.Messages
                .Where(m => m.SessionId == id && m.ByConsultee && m.SentTime < lastConsultantMessageTime)
                .Sum(m => (int?)m.Text.Length) ?? 0;

            double consultantCharCount = _dataContext.Messages
                .Where(m => m.SessionId == id && !m.ByConsultee)
                .Sum(m => (int?)m.Text.Length) ?? 0;

            var writeCoefficient = _settingsService.GetSetting<double>(new SettingEntryKey {Category = "RahnemunSession", Name = "WriteCoefficient" });
            var readCoefficient = _settingsService.GetSetting<double>(new SettingEntryKey { Category = "RahnemunSession", Name = "ReadCoefficient" });

            return (int)Math.Ceiling((consultantCharCount * writeCoefficient + consulteeCharCount * readCoefficient) / 60D);
        }
    }
}