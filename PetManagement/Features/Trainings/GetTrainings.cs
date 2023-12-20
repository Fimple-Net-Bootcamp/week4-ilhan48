using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;

namespace PetManagement.Features.Trainings;

public class GetTrainings
{
    public class Query : IRequest<List<TrainingResponse>>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, List<TrainingResponse>>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainingResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var trainingResponse = await _context
                .Trainings
                .Select(training => new TrainingResponse
                {
                    Name = training.Name,

                })
                .ToListAsync(cancellationToken);

            return trainingResponse;

        }
    }
}

public class GetTrainingsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/trainings", async (ISender sender) =>
        {
            var query = new GetTrainings.Query();

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}