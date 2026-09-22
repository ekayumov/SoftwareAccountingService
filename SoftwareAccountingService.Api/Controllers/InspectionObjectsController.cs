using Microsoft.AspNetCore.Mvc;
using SoftwareAccountingService.Api.Domain.Enums;
using SoftwareAccountingService.Api.DTO;
using SoftwareAccountingService.Api.Services;

namespace SoftwareAccountingService.Api.Controllers
{
    [ApiController]
    [Route("api/inspection-objects")]
    public class InspectionObjectsController : ControllerBase
    {
        private readonly IInspectionObjectService _inspectionObjectService;

        public InspectionObjectsController(
            IInspectionObjectService inspectionObjectService)
        {
            _inspectionObjectService = inspectionObjectService;
        }

        [HttpGet]
        public async Task<ActionResult<List<InspectionObjectDto>>> GetAll(
            [FromQuery] string? search,
            [FromQuery] InspectionResult? result,
            [FromQuery] InspectionType? type,
            CancellationToken cancellationToken)
        {
            List<InspectionObjectDto> inspectionObjects =
                await _inspectionObjectService.GetAllAsync(cancellationToken, search, result, type);
            return Ok(inspectionObjects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InspectionObjectDto>> GetById(
            [FromQuery] Guid id,
            CancellationToken cancellationToken)
        {
            InspectionObjectDto? inspectionObject =
                await _inspectionObjectService.GetByIdAsync(id, cancellationToken);
            if (inspectionObject == null) { return NotFound(); }
            return Ok(inspectionObject);
        }

        [HttpPost]
        public async Task<ActionResult<InspectionObjectDto>> Create(
            [FromBody] CreateInspectionObjectDto dto,
            CancellationToken cancellationToken)
        {
            InspectionObjectDto createdInspectionObject =
                await _inspectionObjectService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = createdInspectionObject.Id }, createdInspectionObject);
        }

        [HttpPatch("{id}/result")]
        public async Task<ActionResult<InspectionObjectDto>> UpdateResult(
            [FromRoute] Guid id,
            [FromBody] UpdateInspectionResultDto dto,
            CancellationToken cancellationToken)
        {
            InspectionObjectDto? inspectionObject =
                await _inspectionObjectService.UpdateResultAsync(id, dto, cancellationToken);
            if (inspectionObject == null) { return NotFound(); }
            return Ok(inspectionObject);
        }

        [HttpGet("filter-options")]
        public ActionResult<InspectionFilterOptionsDto> GetFilterOptions()
        {
            InspectionFilterOptionsDto options =
                _inspectionObjectService.GetFilterOptions();

            return Ok(options);
        }
    }
}
