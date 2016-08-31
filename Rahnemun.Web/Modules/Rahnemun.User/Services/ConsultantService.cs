using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Injection;
using Rahnemun.CategoryContracts;
using Rahnemun.User.Models;
using Rahnemun.UserContracts;
using Rahnemun.Domain;
using IUserService = Edreamer.Framework.Security.Users.IUserService;

namespace Rahnemun.User.Services
{
    public class ConsultantService : IConsultantService
    {

        private readonly IRahnemunDataContext _dataContext;
        private readonly IUserService _frameworkUserService;
        private readonly ICategoryService _categoryService;

        public ConsultantService(IRahnemunDataContext dataContext, IUserService frameworkUserService, ICategoryService categoryService)
        {
            _dataContext = dataContext;
            _frameworkUserService = frameworkUserService;
            _categoryService = categoryService;
        }

        public IQueryable<ConsultantModel> Consultants(int? categoryId)
        {
            var consultantsQuery = _dataContext.Consultants.Where(c => true);
            if (categoryId != null)
                consultantsQuery = consultantsQuery.Where(c => c.Categories.Any(cat => cat.Id == categoryId));

            return consultantsQuery.Select(c => new ConsultantModel
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
                ProfilePictureId = c.ProfilePictureId,
                Title = c.Title,
                BankCardNo = c.BankCardNo,
                BankAccountNo = c.BankAccountNo,
                BankName = c.BankName,
                Education = c.Education,
                ProfessionalExperience = c.ProfessionalExperience,
                LicenseNumber = c.LicenseNumber,
                ProfessionalCertificates = c.ProfessionalCertificates,
                WorkAddress = c.WorkAddress,
                WorkPhoneNo = c.WorkPhoneNo,
                Capacity = c.Capacity,
                Approved = c.Approved,
                Fee = c.Fee
            });
        }

        public ConsultantModel GetConsultant(int id)
        {
            return Consultants(null).SingleOrDefault(c => c.Id == id);
        }

        public ConsultantModel GetConsultant(int id, out bool hasNewData)
        {
            hasNewData = false;
            var consultant = _dataContext.Consultants.SingleOrDefault(c => c.Id == id);
            if (consultant == null) return null;
            if (consultant.ConsultantNewData != null)
            {
                var consultantNewData = DeserializeConsultant(consultant.ConsultantNewData);
                consultantNewData.Id = consultant.Id;
                consultantNewData.Timestamp = consultant.Timestamp;
                consultantNewData.Approved = consultant.Approved;
                hasNewData = true;
                return Injector.PlaneInject(new ConsultantModel(), consultantNewData);
            }
            
            return Consultants(null).SingleOrDefault(c => c.Id == id);
        }

        public IQueryable<CategoryModel> GetConsultantCategories(int id, bool newData = false)
        {
            var consultant = _dataContext.Consultants.SingleOrDefault(c => c.Id == id);
            Throw.IfNull(consultant)
                .AnArgumentException($"No consultant with the id {id} exists.");

            IEnumerable<int> categoriesIds;
            if (newData && consultant.ConsultantNewData != null)
            {
                var consultantNewData = DeserializeConsultant(consultant.ConsultantNewData);
                categoriesIds = consultantNewData.Categories.Select(c => c.Id);
            }
            else
            {
                categoriesIds = _dataContext.Consultants
                    .Where(c => c.Id == id)
                    .SelectMany(c => c.Categories)
                    .Select(cat => cat.Id);
            }

            return _categoryService.Categories
                .Where(c => categoriesIds.Contains(c.Id));
        }

        public int PreliminaryRegisterConsultant(ConsultantPreliminaryRegisterModel model)
        {
            var user = new Edreamer.Framework.Security.User
            {
                Email = model.Email,
                Username = model.Email,
                Approved = true,
                EmailConfirmed = false,
                Disabled = false,
                UserData = new ConsultantPreliminaryData { FirstName = model.FirstName, LastName = model.LastName, SubscribedToNewsletter = model.SubscribedToNewsletter }
            };
            _frameworkUserService.AddUser(user, model.Password);
            return user.Id;
        }

        public void FinalRegisterConsultant(ConsultantUpdateModel consultant)
        {
            var consultantEntity = Injector.PlaneInject(new Consultant(), consultant);
            consultantEntity.RegisterDate = DateTime.UtcNow;
            consultantEntity.Categories.AddRange(CollectionHelpers.EmptyIfNull(consultant.CategoriesIds).Select(id => new Category { Id = id }));
            foreach (var category in consultantEntity.Categories)
            {
                _dataContext.Categories.Attach(category); // Attaching stub categories prevents putting them in added state
            }
            _dataContext.Consultants.Add(consultantEntity);
            _dataContext.SaveChanges();

            var userAccount = _frameworkUserService.GetUser(consultant.Id);
            userAccount.UserData = null;
            _frameworkUserService.UpdateUser(userAccount);

            consultant.Id = consultantEntity.Id;
            consultant.Timestamp = consultantEntity.Timestamp;
        }

        public void UpdateConsultant(ConsultantUpdateModel consultant, bool newData = false)
        {
            if (!newData)
                throw new System.NotImplementedException();

            // NewData
            var consultantEntity = Injector.PlaneInject(new Consultant(), consultant);
            consultantEntity.Categories.AddRange(CollectionHelpers.EmptyIfNull(consultant.CategoriesIds).Select(cid => new Category { Id = cid }));
            consultantEntity.ConsultantNewData = SerializeConsultant(consultantEntity);

            _dataContext.Consultants.Update(consultantEntity, c => new { c.ConsultantNewData });

            _dataContext.SaveChanges();
            consultant.Timestamp = consultantEntity.Timestamp;
        }

        public void DeleteConsultant(int id, byte[] timestamp)
        {
            throw new System.NotImplementedException();
        }

        public bool IsConsultant(int id)
        {
            return _dataContext.Consultants.Any(c => c.Id == id);
        }


        #region Consultant Serialization

        private static string SerializeConsultant(Consultant consultant)
        {
            using (var writer = new StringWriter())
            {
                var consultantSerializable = Injector.PlaneInject(new ConsultantSerializable(), consultant);
                Serializer.Serialize(writer, consultantSerializable);
                return writer.ToString();
            }
        }

        private static Consultant DeserializeConsultant(string serializedConsultant)
        {
            using (var reader = new StringReader(serializedConsultant))
            {
                var consultantSerializable = (ConsultantSerializable)Serializer.Deserialize(reader);
                return Injector.PlaneInject(new Consultant(), consultantSerializable);
            }
        }

        private static XmlSerializer Serializer
        {
            get
            {
                var attributes = new XmlAttributes { XmlIgnore = true };
                var overrides = new XmlAttributeOverrides();
                overrides.Add(typeof(Domain.User), "Id", attributes);
                overrides.Add(typeof(Domain.User), "Timestamp", attributes);
                overrides.Add(typeof(Domain.Consultant), "Approved", attributes);
                // "Categories" is of type ICollection and XmlSerializer cannot serialize an interface
                overrides.Add(typeof(Domain.Consultant), "Categories", attributes);
                return new XmlSerializer(typeof(ConsultantSerializable), overrides);
            }
        }

        // Only public classes can be serialized 
        public class ConsultantSerializable : Consultant
        {
            public int[] CategoriesIds
            {
                get { return Categories.Select(c => c.Id).ToArray(); }
                set
                {
                    if (value != null)
                        Categories = new HashSet<Category>(value.Select(cid => new Category { Id = cid }));
                }
            }
        } 

        #endregion
    }
}