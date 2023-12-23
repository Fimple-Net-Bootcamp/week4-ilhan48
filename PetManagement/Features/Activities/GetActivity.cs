using Carter;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Database.Repositories.ActivityRepository;

namespace PetManagement.Features.Activities;

public class GetActivity
{
    public class Query : IRequest<ActivityResponse>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, ActivityResponse>
    {
        private readonly IActivityRepository _activityRepository;

        public Handler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<ActivityResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var a = _activityRepository.Get(a => a.Id == request.Id);
            var activityResponse = new ActivityResponse
            {
                Name = a.Name,
                DifficultyLevel = a.DifficultyLevel,
                Description = a.Description,
            };

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
