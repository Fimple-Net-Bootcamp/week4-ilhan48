using Carter;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Database.Repositories.ActivityRepository;
using PetManagement.Entities;

namespace PetManagement.Features.Activities;

public class GetActivities
{
    public class Query : IRequest<List<ActivityResponse>>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, List<ActivityResponse>>
    {
        private readonly IActivityRepository _activityRepository;

        public Handler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<List<ActivityResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<Activity> activities = new List<Activity>();

            foreach (var activity in _activityRepository.GetAll())
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