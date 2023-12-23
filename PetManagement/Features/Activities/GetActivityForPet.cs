using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;
using PetManagement.Entities;

namespace PetManagement.Features.Activities;

public class GetActivityForPet
{
    public class Query : IRequest<List<ActivityResponse>>
    {
        public int PetId { get; set; }
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
            var pet = await _context.Pets
             .Include(p => p.Activities)
             .FirstOrDefaultAsync(p => p.Id == request.PetId);

            List<Activity> activities = new List<Activity>();

            foreach (var activity in pet.Activities)
            {
                activities.Add(activity);
            }

            List<ActivityResponse> responses = new List<ActivityResponse>();
            for (int i = 0; i < activities.Count; i++)
            {
                var activityResponse = new ActivityResponse
                {
                    Name = activities[i].Name,
                    Description = activities[i].Description,
                    DifficultyLevel = activities[i].DifficultyLevel,

                };
                responses.Add(activityResponse);
            }

            return responses;
        }
    }
}

public class GetActivityForPetEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/{pet-id}/activities/", async (int petId, ISender sender) =>
        {
            var query = new GetActivityForPet.Query { PetId = petId };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
