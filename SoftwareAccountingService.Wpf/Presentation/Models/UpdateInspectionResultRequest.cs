namespace SoftwareAccountingService.Wpf.Presentation.Models
{
    public sealed class UpdateInspectionResultRequest
    {
        public string Result { get; set; } = string.Empty;

        public string? Note { get; set; }
    }
}
