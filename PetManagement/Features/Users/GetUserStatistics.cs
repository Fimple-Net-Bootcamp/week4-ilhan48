using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;
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
        private readonly PetManagementDbContext _context;
        private readonly ISender _mediator;

        public Handler(PetManagementDbContext context, ISender mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<UserStatistics> Handle(Query request, CancellationToken cancellationToken)
        {
            var petStatisticsQuery = new GetPetStatistics.Query { UserId = request.UserId };

            var userPets = await _context.Pets
                .Where(pet => pet.OwnerId == request.UserId)
                .Select(pet => new PetResponse
                {
                    Name = pet.Name,
                    Type = pet.Type,
                    BirthDate = pet.BirthDate,
                    Color = pet.Color,
                    Gender = pet.Gender,
                })
                .ToListAsync(cancellationToken);

            var userStatistics = new UserStatistics
            {
                UserId = request.UserId,
                OwnedPets = userPets,
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

