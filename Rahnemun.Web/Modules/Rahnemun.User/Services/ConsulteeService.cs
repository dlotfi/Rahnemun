using System;
using System.Linq;
using Edreamer.Framework.Injection;
using Rahnemun.CategoryContracts;
using Rahnemun.User.Models;
using Rahnemun.UserContracts;
using Rahnemun.Domain;
using IUserService = Edreamer.Framework.Security.Users.IUserService;

namespace Rahnemun.User.Services
{
    public class ConsulteeService : IConsulteeService
    {

        private readonly IRahnemunDataContext _dataContext;
        private readonly IUserService _frameworkUserService;

        public ConsulteeService(IRahnemunDataContext dataContext, IUserService frameworkUserService)
        {
            _dataContext = dataContext;
            _frameworkUserService = frameworkUserService;
        }

        public IQueryable<ConsulteeModel> Consultees
        {
            get
            {
                return _dataContext.Users.Select(c => new ConsulteeModel
                {
                    Id = c.Id,
                    Timestamp = c.Timestamp,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Gender = c.Gender,
                    EducationLevel = c.EducationLevel,
                    MaritalStatus = c.MaritalStatus,
                    CellphoneNo = c.CellphoneNo,
                    BirthDate = c.BirthDate,
                    More = c.More,
                    RegisterDate = c.RegisterDate,
                    ProfilePictureId = c.ProfilePictureId
                });
            }
        }

        public ConsulteeModel GetConsultee(int id)
        {
            return Consultees.SingleOrDefault(c => c.Id == id);
        }

        public void AddConsultee(ConsulteeUpdateModel consultee)
        {
            var user = new Edreamer.Framework.Security.User
            {
                Email = consultee.Email,
                Username = consultee.Email,
                Approved = true,
                EmailConfirmed = false,
                Disabled = false
            };
            _frameworkUserService.AddUser(user, consultee.Password);

            //add consultee part
            consultee.Id = user.Id;
            var userEntity = Injector.PlaneInject(new Domain.User(), consultee);
            userEntity.LastName = userEntity.LastName ?? ""; //last name is mandatory
            userEntity.RegisterDate = DateTime.UtcNow;
            _dataContext.Users.Add(userEntity);
            _dataContext.SaveChanges();

            consultee.Timestamp = userEntity.Timestamp;
        }

        public void UpdateConsultee(ConsulteeUpdateModel consultee)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteConsultee(int id, byte[] timestamp)
        {
            throw new System.NotImplementedException();
        }
    }
}