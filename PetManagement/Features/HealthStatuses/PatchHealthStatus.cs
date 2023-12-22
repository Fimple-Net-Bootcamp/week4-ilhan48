using Carter;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PetManagement.Contracts;
using PetManagement.Database;

namespace PetManagement.Features.HealthStatuses;

public static class PatchHealthStatus
{
    public class PatchHealthStatusCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public DateTime ExaminationDate { get; set; }
        public bool VaccinationStatus { get; set; }
        public string DiseaseStatus { get; set; }
        public string TreatmentInfo { get; set; }
        public string Notes { get; set; }
        public int PetId { get; set; }
    }

    internal sealed class Handler : IRequestHandler<PatchHealthStatusCommand, Unit>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PatchHealthStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var healthStatus = await _context.HealthStatuses.FindAsync(request.Id);

                if (healthStatus != null)
                {
                    healthStatus.VaccinationStatus = request.VaccinationStatus;
                    healthStatus.DiseaseStatus = request.DiseaseStatus;
                    healthStatus.Notes = request.Notes;
                    healthStatus.ExaminationDate = request.ExaminationDate;
                    healthStatus.TreatmentInfo = request.TreatmentInfo;
                    healthStatus.PetId = request.PetId;
                    healthStatus.UpdatedDate = DateTime.UtcNow;

                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateHealthStatus.Handler: {ex.Message}");
                throw;
            }
        }
    }
}

[ApiController]
[Route("api/v1/healthstatuses")]
public class HealthStatusController : ControllerBase
{
    private readonly PetManagementDbContext _context;

    public HealthStatusController(PetManagementDbContext context)
    {
        _context = context;
    }

    [HttpPatch("api/v1/healthstatuses/{id}")]
    public async Task<IActionResult> PatchHealthStatus(int id, [FromBody] JsonPatchDocument<CreateHealthStatusRequest> patchDocument)
    {
        if (patchDocument == null)
        {
            return BadRequest();
        }

        var healthStatus = await _context.HealthStatuses.FindAsync(id);

        if (healthStatus == null)
        {
            return NotFound();
        }

        var patchDto = new CreateHealthStatusRequest
        {
            ExaminationDate = healthStatus.ExaminationDate,
            VaccinationStatus = healthStatus.VaccinationStatus,
            DiseaseStatus = healthStatus.DiseaseStatus,
            TreatmentInfo = healthStatus.TreatmentInfo,
            Notes = healthStatus.Notes,
            PetId = healthStatus.PetId
        };

        patchDocument.ApplyTo(patchDto, ModelState);

        if (!TryValidateModel(patchDto))
        {
            return BadRequest(ModelState);
        }

        healthStatus.ExaminationDate = patchDto.ExaminationDate;
        healthStatus.VaccinationStatus = patchDto.VaccinationStatus;
        healthStatus.DiseaseStatus = patchDto.DiseaseStatus;
        healthStatus.TreatmentInfo = patchDto.TreatmentInfo;
        healthStatus.Notes = patchDto.Notes;
        healthStatus.PetId = patchDto.PetId;
        healthStatus.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
