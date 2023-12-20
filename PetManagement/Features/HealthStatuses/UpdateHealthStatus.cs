using Carter;
using MediatR;
using PetManagement.Database;
using PetManagement.Features.Pets;

namespace PetManagement.Features.HealthStatuses;

public static class UpdateHealthStatus
{
    public class UpdateHealthStatusCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public DateTime ExaminationDate { get; set; }
        public bool VaccinationStatus { get; set; }
        public string DiseaseStatus { get; set; }
        public string TreatmentInfo { get; set; }
        public string Notes { get; set; }
        public int PetId { get; set; }
    }

    internal sealed class  Handler : IRequestHandler<UpdateHealthStatusCommand, Unit>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateHealthStatusCommand request, CancellationToken cancellationToken)
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


                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdatePet.Handler: {ex.Message}");
                throw;
            }
        }
    }
}

public class UpdateHealthStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/healthStatuses/{id}", async (int id, UpdateHealthStatus.UpdateHealthStatusCommand request, ISender sender) =>
        {
            request.Id = id;

            await sender.Send(request);

            return Results.NoContent();
        });
    }
}