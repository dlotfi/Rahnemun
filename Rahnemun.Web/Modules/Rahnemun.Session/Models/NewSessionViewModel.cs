using System.ComponentModel.DataAnnotations;

namespace Rahnemun.Session.Models
{
    public class NewSessionViewModel
    {
        public string ConsulteeFullName { get; set; }
        public string ConsulteeEmail { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public int ConsultantId { get; set; }
        public string ConsultantFullName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryCaption { get; set; }
        public string CategoryTerms { get; set; }
        [DataType(DataType.Currency)]
        public decimal SessionFee { get; set; }
        [DataType(DataType.Currency)]
        public decimal? SessionFeeWithoutDiscount { get; set; }
    }
}