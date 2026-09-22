using Microsoft.EntityFrameworkCore;
using SoftwareAccountingService.Api.Data;
using SoftwareAccountingService.Api.Domain.Entities;
using SoftwareAccountingService.Api.Domain.Enums;
using SoftwareAccountingService.Api.DTO;
using SoftwareAccountingService.Api.Mappings;



namespace SoftwareAccountingService.Api.Services
{
    public class InspectionObjectService : IInspectionObjectService
    {
        private readonly ApplicationDbContext _dbContext;

        public InspectionObjectService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<InspectionObjectDto>> GetAllAsync(
            CancellationToken cancellationToken,
            string? search,
            InspectionResult? result,
            InspectionType? type)
        {
            IQueryable<InspectionObject> query = _dbContext.InspectionObjects.AsNoTracking();
            if (search != null) 
            { 
                string searchText = search.Trim();
                query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchText}%"));
            }
            if (type != null) { query = query.Where(x => x.Type == type.Value); }
            if (result != null) { query = query.Where(x => x.Result == result.Value); }

            return await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new InspectionObjectDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Version = x.Version,
                    Type = x.Type,
                    Result = x.Result,
                    ReceivedDate = x.ReceivedDate,
                    Note = x.Note
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<InspectionObjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            InspectionObject? result = 
                await _dbContext.InspectionObjects.AsNoTracking()
                    .FirstOrDefaultAsync(x=> x.Id == id, cancellationToken);
            if (result == null) { return null; }

            return new InspectionObjectDto
            {
                Id = result.Id,
                Name = result.Name,
                Version = result.Version,
                Type = result.Type,
                Result = result.Result,
                ReceivedDate = result.ReceivedDate,
                Note = result.Note
            };
        }

        public async Task<InspectionObjectDto?> UpdateResultAsync(Guid id, UpdateInspectionResultDto dto, CancellationToken cancellationToken)
        {
            DateTime currentDate = DateTime.UtcNow;
            InspectionObject? result =
                await _dbContext.InspectionObjects
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (result == null) { return null; }

            result.Result = dto.Result!.Value;
            result.Note = dto.Note;
            result.LastUpdatedAt = currentDate;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new InspectionObjectDto
            {
                Id = result.Id,
                Name = result.Name,
                Version = result.Version,
                Type = result.Type,
                Result = result.Result,
                ReceivedDate = result.ReceivedDate,
                Note = result.Note
            };

        }

        public async Task<InspectionObjectDto> CreateAsync(CreateInspectionObjectDto dto, CancellationToken cancellationToken)
        {
            DateTime currentDate = DateTime.UtcNow;

            InspectionObject result = new InspectionObject
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Version = dto.Version.Trim(),
                Type = dto.Type!.Value,
                Result = InspectionResult.InProgress,
                ReceivedDate = dto.ReceivedDate!.Value,
                Note = dto.Note,
                CreatedAt = currentDate,
                LastUpdatedAt = currentDate

            };

            _dbContext.InspectionObjects.Add(result);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new InspectionObjectDto
            {
                Id = result.Id,
                Name = result.Name,
                Version = result.Version,
                Type = result.Type,
                Result = result.Result,
                ReceivedDate = result.ReceivedDate,
                Note = result.Note
            };
        }

        public InspectionFilterOptionsDto GetFilterOptions()
        {
            return InspectionFilterOptionsFactory.Create();
        }
    }
}
