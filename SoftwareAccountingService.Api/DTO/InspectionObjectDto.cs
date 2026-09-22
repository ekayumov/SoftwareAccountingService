using SoftwareAccountingService.Api.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SoftwareAccountingService.Api.DTO
{
    public class InspectionObjectDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public string Version { get; set; } = "";

        public InspectionType Type { get; set; }

        public InspectionResult Result { get; set; }

        public DateTime ReceivedDate { get; set; }

        public string Note { get; set; } = "";
    }
}
