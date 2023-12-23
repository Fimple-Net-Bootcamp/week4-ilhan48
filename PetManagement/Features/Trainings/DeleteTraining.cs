using Carter;
using MediatR;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.TrainingRepository;

namespace PetManagement.Features.Trainings;

public static class DeleteTraining
{
    public class Command : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Unit>
    {
        private readonly ITrainingRepository _trainingRepository;

        public Handler(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var training = _trainingRepository.Get(t => t.Id == request.Id);

            if (training != null)
            {
                _trainingRepository.Delete(training);
            }

            return Unit.Value;
        }
    }
}
public class DeleteTrainingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/trainings/{id}", async (int id, ISender sender) =>
        {
            var command = new DeleteTraining.Command { Id = id };

            await sender.Send(command);

            return Results.NoContent();
        });
    }
}
