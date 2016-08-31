namespace Rahnemun.UserContracts
{
    public class ConsultantPreliminaryRegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool SubscribedToNewsletter { get; set; }
    }
}