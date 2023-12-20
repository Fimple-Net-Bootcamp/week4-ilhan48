using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;

namespace PetManagement.Features.Activities;

public class GetActivity
{
    public class Query : IRequest<ActivityResponse>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, ActivityResponse>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<ActivityResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var activityResponse = await _context
                .Activities
                .Where(a => a.Id == request.Id)
                .Select(a => new ActivityResponse
                {
                    Name = a.Name,
                    DifficultyLevel = a.DifficultyLevel,
                    Description = a.Description,
                })
                .FirstOrDefaultAsync(cancellationToken);

            return activityResponse;
        }
    }
}

public class GetActivityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/activities/{id}", async (int id, ISender sender) =>
        {
            var query = new GetActivity.Query { Id = id };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
