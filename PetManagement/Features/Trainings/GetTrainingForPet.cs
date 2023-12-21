using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;

namespace PetManagement.Features.Trainings;

public class GetTrainingForPet
{
    public class Query : IRequest<List<TrainingResponse>>
    {
        public int PetId { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, List<TrainingResponse>>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainingResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var pet = await _context.Pets
            .Include(p => p.Trainings)
            .FirstOrDefaultAsync(p => p.Id == request.PetId);

            List<Training> trainings = new List<Training>();

            foreach (var training in pet.Trainings)
            {
                trainings.Add(training);
            }

            List<TrainingResponse> responses = new List<TrainingResponse>();
            for (int i = 0; i < trainings.Count; i++)
            {
                var trainingResponse = new TrainingResponse
                {
                    Name = trainings[i].Name,
                    Date = trainings[i].Date,
                };
                responses.Add(trainingResponse);
            }

            return responses;
        }
    }
}

public class GetTrainingForPetEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/{pet-id}/trainings", async (int petId, ISender sender) =>
        {
            var query = new GetTrainingForPet.Query { PetId = petId };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}

