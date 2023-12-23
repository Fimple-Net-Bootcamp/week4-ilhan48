using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.TrainingRepository;

namespace PetManagement.Features.Trainings;

public class GetTraining
{
    public class Query : IRequest<TrainingResponse>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, TrainingResponse>
    {
        private readonly ITrainingRepository _trainingRepository;

        public Handler(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }

        public async Task<TrainingResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var training = _trainingRepository.Get(t => t.Id == request.Id);
            var trainingResponse = new TrainingResponse
            {
                Name = training.Name,
                Date = training.Date,

            };

            return trainingResponse;
        }
    }
}

public class GetTrainingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/training/{id}", async (int id, ISender sender) =>
        {
            var query = new GetTraining.Query { Id = id };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
