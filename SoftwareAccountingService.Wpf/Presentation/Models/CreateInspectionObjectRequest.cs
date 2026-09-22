namespace SoftwareAccountingService.Wpf.Presentation.Models
{
    public sealed class CreateInspectionObjectRequest
    {
        public string Name { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public DateTime ReceivedDate { get; set; }

        public string? Note { get; set; }
    }
}
