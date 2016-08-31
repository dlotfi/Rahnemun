namespace Rahnemun.UIContracts
{
    public class UIInfo
    {
        public string NavigationId { get; set; }
        public object NavigationData { get; set; }
        public string PageTitle { get; set; }
        public string PageSubtitle { get; set; }
        public bool ShowMenu { get; set; }
        public bool NoBanner { get; set; }
    }
}