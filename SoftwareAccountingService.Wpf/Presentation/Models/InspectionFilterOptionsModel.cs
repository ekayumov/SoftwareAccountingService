namespace SoftwareAccountingService.Wpf.Presentation.Models
{
    public sealed class InspectionFilterOptionsModel
    {
        public List<FilterOptionModel> Types { get; set; }
            = new List<FilterOptionModel>();

        public List<FilterOptionModel> Results { get; set; }
            = new List<FilterOptionModel>();
    }
}
