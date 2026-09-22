using SoftwareAccountingService.Api.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SoftwareAccountingService.Api.DTO
{
    public class CreateInspectionObjectDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Version { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(InspectionType))]
        public InspectionType? Type { get; set; }

        [Required]
        public DateTime? ReceivedDate { get; set; }

        [MaxLength(1000)]
        public string? Note { get; set; }
    }
}