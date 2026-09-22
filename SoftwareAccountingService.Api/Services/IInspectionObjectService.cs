using SoftwareAccountingService.Api.Domain.Emuns;
using SoftwareAccountingService.Api.DTO;

namespace SoftwareAccountingService.Api.Services
{
    public interface IInspectionObjectService
    {
        Task<List<InspectionObjectDto>> GetAllAsync(
            CancellationToken cancellationToken,
            string? search,
            InspectionResult? result,
            InspectionType? type);

        Task<InspectionObjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<InspectionObjectDto> CreateAsync(CreateInspectionObjectDto dto, CancellationToken cancellationToken);

        Task<InspectionObjectDto> UpdateResultAsync(Guid id, UpdateInspectionResultDto dto, CancellationToken cancellationToken);

        InspectionFilterOptionsDto GetFilterOptions();
    }
}
