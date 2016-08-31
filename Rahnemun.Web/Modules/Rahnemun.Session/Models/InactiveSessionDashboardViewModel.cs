namespace Rahnemun.Session.Models
{
    public class InactiveSessionDashboardViewModel: ActiveSessionDashboardViewModel
    {
        public string StopTime { get; set; }
        public string StopReason { get; set; }
    }
}