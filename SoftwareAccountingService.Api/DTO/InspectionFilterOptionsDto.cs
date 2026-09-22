namespace SoftwareAccountingService.Api.DTO
{
    public sealed class InspectionFilterOptionsDto
    {
        public List<FilterOptionDto> Types { get; set; } = new();

        public List<FilterOptionDto> Results { get; set; } = new();
    }
}
