using Carter;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Features.Pets;

namespace PetManagement.Features.Users;

public static class GetUserStatistics
{
    public class Query : IRequest<UserStatistics>
    {
        public Guid UserId { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, UserStatistics>
    {
        private readonly ISender _mediator;

        public Handler(ISender mediator)
        {
            _mediator = mediator;
        }

        public async Task<UserStatistics> Handle(Query request, CancellationToken cancellationToken)
        {
            var petStatisticsQuery = new GetPetStatistics.Query { UserId = request.UserId };

            var userStatistics = new UserStatistics
            {
                UserId = request.UserId,
                PetStatistics = await _mediator.Send(petStatisticsQuery),
            };

            return userStatistics;
        }
    }
}

public class GetUserStatisticsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/users/statistics/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetUserStatistics.Query { UserId = id };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}

