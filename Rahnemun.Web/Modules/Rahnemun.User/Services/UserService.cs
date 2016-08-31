using System;
using System.Linq;
using Edreamer.Framework.Data;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Injection;
using Rahnemun.Common;
using Rahnemun.Domain;
using Rahnemun.UserContracts;

namespace Rahnemun.User
{
    public class UserService : IUserService
    {
        private readonly IRahnemunDataContext _dataContext;
        private readonly IConsultantService _consultantService;

        public UserService(IRahnemunDataContext dataContext, IConsultantService consultantService)
        {
            _dataContext = dataContext;
            _consultantService = consultantService;
        }

        public IQueryable<UserModel> Users
        {
            get
            {
                return _dataContext.Users.Select(u => new UserModel
                                                      {
                                                          Id = u.Id,
                                                          Timestamp = u.Timestamp,
                                                          FirstName = u.FirstName,
                                                          LastName = u.LastName,
                                                          Gender = u.Gender,
                                                          EducationLevel = u.EducationLevel,
                                                          MaritalStatus = u.MaritalStatus,
                                                          CellphoneNo = u.CellphoneNo,
                                                          BirthDate = u.BirthDate,
                                                          SubscribedToNewsletter = u.SubscribedToNewsletter,
                                                          More = u.More,
                                                          RegisterDate = u.RegisterDate,
                                                          ProfilePictureId = u.ProfilePictureId
                                                      });
            }
        }

        public UserModel GetUser(int id)
        {
            return Users.SingleOrDefault(c => c.Id == id);
        }

        public void AddUser(UserUpdateModel user)
        {
            var userEntity = Injector.PlaneInject(new Domain.User(), user);
            _dataContext.Users.Add(userEntity);
            _dataContext.SaveChanges();
            user.Id = userEntity.Id;
            user.Timestamp = userEntity.Timestamp;
        }

        public void UpdateUser(UserUpdateModel user)
        {
            var userEntity = Injector.PlaneInject(new Domain.User(), user);
            _dataContext.Users.Update(userEntity, u => new { u.RegisterDate }, PropertyInclusion.Exclude);
            _dataContext.SaveChanges();
            user.Timestamp = userEntity.Timestamp;
        }

        public void DeleteUser(int id, byte[] timestamp)
        {
            throw new NotImplementedException();
        }

        public string GetUserFullName(UserModel user, bool withTitle = false)
        {
            Throw.IfArgumentNull(user, "user");

            var name = user.FirstName + (String.IsNullOrWhiteSpace(user.LastName) ? "" : (" " + user.LastName));

            // When user is consultee
            if (!_consultantService.IsConsultant(user.Id))
                return name;

            // When user is consultant
            var courtesyTitle = "";
            if (withTitle)
            {
                switch (user.Gender)
                {
                    case Gender.Male:
                        courtesyTitle = "جناب آقای ";
                        break;
                    case Gender.Female:
                        courtesyTitle = "سرکار خانم ";
                        break;
                }
            }
            var proffesionalTitle = user.EducationLevel == EducationLevel.Doctorate ? "دکتر " : "";
            return courtesyTitle + proffesionalTitle + name;
        }
    }
}