using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;

namespace PetManagement.Features.Activities;

public class GetActivities
{
    public class Query : IRequest<List<ActivityResponse>>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, List<ActivityResponse>>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<ActivityResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activityResponse = await _context
                .Activities
                .Select(a => new ActivityResponse
                {
                    Name = a.Name,
                    Description = a.Description,
                    DifficultyLevel = a.DifficultyLevel,
                })
                .ToListAsync(cancellationToken);

            return activityResponse;

        }
    }
}

public class GetActivitiesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/activities", async (ISender sender) =>
        {
            var query = new GetActivities.Query();

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}