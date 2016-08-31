using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rahnemun.Common;

namespace Rahnemun.Domain
{
    public class Consultant : User
    {
        public Consultant()
        {
            Categories = new HashSet<Category>();
        }

        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required, MinLength(16), MaxLength(16)]
        public string BankCardNo { get; set; }
        [MaxLength(30)]
        public string BankAccountNo { get; set; }
        [MaxLength(50)]
        public string BankName { get; set; }
        [Required, MaxLength(1000)]
        public string Education { get; set; }
        [Required, MaxLength(1000)]
        public string ProfessionalExperience { get; set; }
        [MaxLength(30)]
        public string LicenseNumber { get; set; }
        [MaxLength(1000)]
        public string ProfessionalCertificates { get; set; }
        [MaxLength(300)]
        public string WorkAddress { get; set; }
        [MaxLength(50)]
        public string WorkPhoneNo { get; set; }
        public byte Capacity { get; set; }
        public bool Approved { get; set; }
        public string ConsultantNewData { get; set; }
        [Currency]
        public decimal Fee { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
