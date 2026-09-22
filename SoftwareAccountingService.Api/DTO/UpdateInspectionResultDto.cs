using SoftwareAccountingService.Api.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SoftwareAccountingService.Api.DTO
{
    public class UpdateInspectionResultDto
    {
        [Required]
        public InspectionResult? Result { get; set; }

        [MaxLength(1000)]
        public string? Note { get; set; }
    }
}
