using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.TrainingRepository;
using PetManagement.Entities;

namespace PetManagement.Features.Trainings;

public class GetTrainings
{
    public class Query : IRequest<List<TrainingResponse>>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, List<TrainingResponse>>
    {
        private readonly ITrainingRepository _trainingRepository;

        public Handler(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }

        public async Task<List<TrainingResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<Training> trainings = new List<Training>();

            foreach (var training in _trainingRepository.GetAll())
            {
                trainings.Add(training);
            }

            List<TrainingResponse> responses = new List<TrainingResponse>();
            for (int i = 0; i < trainings.Count; i++)
            {
                var foodResponse = new TrainingResponse
                {
                    Name = trainings[i].Name,

                };
                responses.Add(foodResponse);
            }

            return responses;

        }
    }
}

public class GetTrainingsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/trainings", async (ISender sender) =>
        {
            var query = new GetTrainings.Query();

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}