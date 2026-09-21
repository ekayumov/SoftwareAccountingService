using SoftwareAccountingService.Api.Domain.Emuns;
using System.ComponentModel.DataAnnotations;

namespace SoftwareAccountingService.Api.Domain.Entities
{
    public class InspectionObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Version { get; set; }
        public InspectionsType Type { get; set; }
        public InspectionsResult Result { get; set; } = InspectionsResult.InProgress;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set;} = DateTime.UtcNow;
        public DateTime ReceivedDate { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; } = "";

    }
}
