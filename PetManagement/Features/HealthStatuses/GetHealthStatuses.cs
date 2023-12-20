using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;

namespace PetManagement.Features.HealthStatuses;

public class GetHealthStatuses
{
    public class Query : IRequest<List<HealthStatusResponse>>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, List<HealthStatusResponse>>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<HealthStatusResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var healthStatusResponse = await _context
                .HealthStatuses
                .Select(hs => new HealthStatusResponse
                {
                    TreatmentInfo = hs.TreatmentInfo,
                    VaccinationStatus = hs.VaccinationStatus,
                    DiseaseStatus = hs.DiseaseStatus,
                    ExaminationDate = hs.ExaminationDate,
                    Notes = hs.Notes,
                    PetId = hs.PetId
                })
                .ToListAsync(cancellationToken);

            return healthStatusResponse;

        }
    }
}

public class GetHealthStatusesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/healthstatuses", async (ISender sender) =>
        {
            var query = new GetHealthStatus.Query();

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}