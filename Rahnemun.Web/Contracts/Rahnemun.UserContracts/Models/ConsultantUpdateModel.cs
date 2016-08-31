using System.Collections.Generic;

namespace Rahnemun.UserContracts
{
    public class ConsultantUpdateModel : UserUpdateModel
    {
        public string Title { get; set; }
        public string BankCardNo { get; set; }
        public string BankAccountNo { get; set; }
        public string BankName { get; set; }
        public string Education { get; set; }
        public string ProfessionalExperience { get; set; }
        public string LicenseNumber { get; set; }
        public string ProfessionalCertificates { get; set; }
        public string WorkAddress { get; set; }
        public string WorkPhoneNo { get; set; }
        public byte Capacity { get; set; }
        public bool Approved { get; set; }
        public IEnumerable<int> CategoriesIds { get; set; }
    }
}
