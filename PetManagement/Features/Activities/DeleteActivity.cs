using Carter;
using MediatR;
using PetManagement.Database.Repositories.ActivityRepository;

namespace PetManagement.Features.Activities;

public static class DeleteActivity
{
    public class Command : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Unit>
    {

        private readonly IActivityRepository _activityRepository;

        public Handler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = _activityRepository.Get(a=> a.Id == request.Id);

            if (activity != null)
            {
                _activityRepository.Delete(activity);
            }

            return Unit.Value;
        }
    }
}
public class DeleteActivityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/activities/{id}", async (int id, ISender sender) =>
        {
            var command = new DeleteActivity.Command { Id = id };

            await sender.Send(command);

            return Results.NoContent();
        });
    }
}
