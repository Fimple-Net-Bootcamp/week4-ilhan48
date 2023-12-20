using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;
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
            var activityQuery = new GetActivity.Query { Id = request.PetId };
            var healthStatusQuery = new GetHealthStatus.Query { Id = request.PetId };
            var foodQuery = new GetFood.Query { Id = request.PetId };

            var petStatistics = new PetStatistics
            {
                PetId = request.PetId,
                ActivityStatistics = await _mediator.Send(activityQuery),
                HealthStatusStatistics = await _mediator.Send(healthStatusQuery),
                FoodStatistics = await _mediator.Send(foodQuery),

            };
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