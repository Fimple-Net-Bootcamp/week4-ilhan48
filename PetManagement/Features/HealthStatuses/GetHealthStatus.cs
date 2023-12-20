using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;

namespace PetManagement.Features.HealthStatuses;

public static class GetHealthStatus
{
    public class Query : IRequest<HealthStatusResponse>
    {
        public int HealthStatusId { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, HealthStatusResponse>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<HealthStatusResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var healthStatusResponse = await _context
                .HealthStatuses
                .Where(hs => hs.Id == request.HealthStatusId)
                .Select(hs => new HealthStatusResponse
                {
                    TreatmentInfo = hs.TreatmentInfo,
                    VaccinationStatus = hs.VaccinationStatus,
                    DiseaseStatus = hs.DiseaseStatus,
                    ExaminationDate = hs.ExaminationDate,
                    Notes = hs.Notes,
                    PetId = hs.PetId
                })
                .FirstOrDefaultAsync(cancellationToken);

            return healthStatusResponse;
        }
    }
}

public class GetHealthStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/healthStatuses/{id}", async (int id, ISender sender) =>
        {
            var query = new GetHealthStatus.Query { HealthStatusId = id };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
