using Carter;
using MediatR;
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

public class PatchHealthStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/v1/healthStatuses/{id}", async (HttpContext context, int id, PatchHealthStatus.PatchHealthStatusCommand request, ISender sender) =>
        {
            request.Id = id;

            await sender.Send(request);

            return Results.NoContent();
        });
    }
}
