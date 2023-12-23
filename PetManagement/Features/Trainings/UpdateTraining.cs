using Carter;
using MediatR;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.TrainingRepository;
using PetManagement.Features.Foods;

namespace PetManagement.Features.Trainings;

public class UpdateTraining
{
    public class UpdateTrainingCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal sealed class Handler : IRequestHandler<UpdateTrainingCommand, Unit>
    {
        private readonly ITrainingRepository _trainingRepository;

        public Handler(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }

        public async Task<Unit> Handle(UpdateTrainingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var training = _trainingRepository.Get(t => t.Id == request.Id);

                if (training != null)
                {
                    training.Name = request.Name;
                    training.UpdatedDate = DateTime.UtcNow;

                    _trainingRepository.Update(training);
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateTraining.Handler: {ex.Message}");
                throw;
            }
        }
    }
}

public class UpdateTrainingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/trainings/{id}", async (int id, UpdateTraining.UpdateTrainingCommand request, ISender sender) =>
        {
            request.Id = id;

            await sender.Send(request);

            return Results.NoContent();
        });
    }
}
