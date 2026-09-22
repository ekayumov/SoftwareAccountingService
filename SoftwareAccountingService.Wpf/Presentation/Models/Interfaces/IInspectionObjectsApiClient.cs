namespace SoftwareAccountingService.Wpf.Presentation.Models.Interfaces
{
    public interface IInspectionObjectsApiClient
    {
        Task<IReadOnlyList<InspectionObjectModel>> GetAllAsync(
            string? search,
            string? type,
            string? result,
            CancellationToken cancellationToken);

        Task<InspectionObjectModel> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<InspectionObjectModel> CreateAsync(
            CreateInspectionObjectRequest request,
            CancellationToken cancellationToken);

        Task<InspectionObjectModel> UpdateResultAsync(
            Guid id,
            UpdateInspectionResultRequest request,
            CancellationToken cancellationToken);

        Task<InspectionFilterOptionsModel> GetFilterOptionsAsync(
            CancellationToken cancellationToken);
    }
}
