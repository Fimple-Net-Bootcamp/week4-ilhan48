using Carter;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Features.Activities;
using PetManagement.Features.Foods;
using PetManagement.Features.HealthStatuses;

namespace PetManagement.Features.Pets;

//public static class GetPetStatistics
//{
//    public class Query : IRequest<PetStatistics>
//    {
//        public int PetId { get; set; }
//    }

//    internal sealed class Handler : IRequestHandler<Query, PetStatistics>
//    {
//        private readonly PetManagementDbContext _context;

//        public Handler(PetManagementDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<PetStatistics> Handle(Query request, CancellationToken cancellationToken)
//        {
//            var petStatistics = new PetStatistics
//            {
//                PetId = request.PetId,
//                ActivityStatistics = await GetActivityEndpoint (request.PetId),
//                HealthStatusStatistics = await Features.HealthStatuses.GetHealthStatus(request.PetId),
//                FoodStatistics = await GetFood(request.PetId),
//            };
//            return petStatistics;
//        }
//    }
//}

public static class GetPetStatistics
{
    public class Query : IRequest<PetStatistics>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, PetStatistics>
    {
        private readonly ISender _mediator;

        public Handler(ISender mediator)
        {
            _mediator = mediator;
        }

        public async Task<PetStatistics> Handle(Query request, CancellationToken cancellationToken)
        {
            var activityQuery = new GetActivity.Query { Id = request.Id };
            var healthStatusQuery = new GetHealthStatus.Query { Id = request.Id };
            var foodQuery = new GetFood.Query { Id = request.Id };

            var petStatistics = new PetStatistics
            {
                PetId = request.Id,
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
            var query = new GetPetStatistics.Query { Id = id };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}