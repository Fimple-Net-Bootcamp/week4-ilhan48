using Carter;
using MediatR;
using PetManagement.Database.Repositories.ActivityRepository;

namespace PetManagement.Features.Activities;

public class UpdateActivity
{
    public class UpdateActivityCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DifficultyLevel { get; set; }
    }

    internal sealed class Handler : IRequestHandler<UpdateActivityCommand, Unit>
    {
        private readonly IActivityRepository _activityRepository;

        public Handler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Unit> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var activity = _activityRepository.Get(a => a.Id == request.Id); 

                if (activity != null)
                {
                    activity.Name = request.Name;
                    activity.Description = request.Description;
                    activity.DifficultyLevel = request.DifficultyLevel;
                    activity.UpdatedDate = DateTime.UtcNow;

                    _activityRepository.Update(activity);
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateActivity.Handler: {ex.Message}");
                throw;
            }
        }
    }
}

public class UpdateActivityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/activities/{id}", async (int id, UpdateActivity.UpdateActivityCommand request, ISender sender) =>
        {
            request.Id = id;

            await sender.Send(request);

            return Results.NoContent();
        });
    }
}