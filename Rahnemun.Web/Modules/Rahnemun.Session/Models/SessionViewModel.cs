using Rahnemun.Common;

namespace Rahnemun.Session.Models
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public SessionParticipationType UserParticipationType { get; set; }

        public string ConsulteeFullName { get; set; }
        public string ConsulteeUrl { get; set; }
        public int? ConsulteeProfilePictureId { get; set; }
        public Gender? ConsulteeGender { get; set; }

        public string ConsultantFullName { get; set; }
        public string ConsultantUrl { get; set; }
        public int? ConsultantProfilePictureId { get; set; }
        public Gender? ConsultantGender { get; set; }

        public string CategoryCaption { get; set; }

        public string SourceUrl { get; set; }
        public string SendUrl { get; set; }
        public string SetSeenUrl { get; set; }
        public string StopUrl { get; set; }
        public string NewSessionUrl { get; set; }

        public bool Stopped { get; set; }
    }
}