using SoftwareAccountingService.Api.Domain.Emuns;

namespace SoftwareAccountingService.Api.Domain.Entities
{
    public class InspectionObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Version { get; set; }
        public InspectionsType Type { get; set; }
        public InspectionsResult Result { get; set; } = InspectionsResult.InProgress;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set;} = DateTime.UtcNow;
        public DateTime ReceivedDate { get; set; }
        public string Note { get; set; } = "";

    }
}
