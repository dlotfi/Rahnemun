using System.Collections.Generic;
using Rahnemun.Common;

namespace Rahnemun.User.Models
{
    public class ConsultantIndexViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryCaption { get; set; }
        public string CategoryGroupCaption { get; set; }
        public IEnumerable<ConsultantSummaryViewModel> ConsultantsSummary { get; set; }

    }

    public class ConsultantSummaryViewModel
    {
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public int? ProfilePictureId { get; set; }
        public string Title { get; set; }
        public string ConsultantProfileUrl { get; set; }
    }
}