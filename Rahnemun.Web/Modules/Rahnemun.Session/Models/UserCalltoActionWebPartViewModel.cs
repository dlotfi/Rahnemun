namespace Rahnemun.Session.Models
{
    public class UserCalltoActionWebPartViewModel
    {
        public int ConsultantId { get; set; }
        public int CategoryId { get; set; }
        public int? ActiveSessionId { get; set; }

        public string ConsultantFullName { get; set; }
        public string VisitorFullName { get; set; } //We dont know he is consultee or consultant

        public string CategoryCaption { get; set; }

        public UserState UserState { get; set; }
    }

    public enum UserState
    {
        Aanonymous,
        PreliminaryConsultant,
        Consultant,
        ConsultantWhoViewHisProfile,
        ConsulteeWithActiveSession,
        ConsulteeWithNoActiveSession
    }
}