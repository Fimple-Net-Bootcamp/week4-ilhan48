using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;
using PetManagement.Entities;
using PetManagement.Features.Activities;
using PetManagement.Features.Foods;
using PetManagement.Features.HealthStatuses;

namespace PetManagement.Features.Pets;

public static class GetPetStatistics
{
    public class Query : IRequest<PetStatistics>
    {
        public int PetId { get; set; }
        public Guid? UserId { get; set; }
        public ICollection<ActivityResponse>? Activities { get; set; }
        public ICollection<FoodResponse>? Foods { get; set; }
        public ICollection<TrainingResponse>? Trainings { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, PetStatistics>
    {
        private readonly PetManagementDbContext _context;
        private readonly ISender _mediator;

        public Handler(PetManagementDbContext context, ISender mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<PetStatistics> Handle(Query request, CancellationToken cancellationToken)
        {
            var petStatistics = new PetStatistics
            {
                PetId = request.PetId,
            };

            
            var pet = await _context.Pets
                .Include(p => p.Activities)
                .Include(p => p.Foods)
                .Include(p => p.Trainings)
                .FirstOrDefaultAsync(p => p.Id == request.PetId);

            if (pet != null)
            {
                petStatistics.Activities = pet.Activities.Select(a => new ActivityResponse
                {
                    Name = a.Name,
                    Description = a.Description,
                    DifficultyLevel = a.DifficultyLevel,
                }).ToList();

                petStatistics.Foods = pet.Foods
                    .Select(a => new FoodResponse
                {
                    Name = a.Name,
                }).ToList();

                

                petStatistics.Trainings = pet.Trainings.Select(a => new TrainingResponse
                {
                    Name = a.Name,
                    Date = a.Date,
                }).ToList();




            }

            return petStatistics;

        }
    }
}

public class GetPetStatisticsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/pets/statistics/{id}", async (int id, ISender sender) =>
        {
            var query = new GetPetStatistics.Query { PetId = id };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}