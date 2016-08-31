using System.ComponentModel.DataAnnotations;

namespace Rahnemun.User.Models
{
    public class UnauthorizedErrorViewModel
    {
        public string ErrorMessage { get; set; }
        public bool IsAuthenticated { get; set; }
        public string SupportEmail { get; set; }
    }
}