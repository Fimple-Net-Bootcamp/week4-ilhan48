using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;

namespace PetManagement.Features.Users;

public static class GetUsers
{
    public class Query : IRequest<List<UserResponse>>
    {

    }

    internal sealed class Handle : IRequestHandler<Query, List<UserResponse>>
    {
        private readonly PetManagementDbContext _context;

        public Handle(PetManagementDbContext context)
        {
            _context = context;
        }

        async Task<List<UserResponse>> IRequestHandler<Query, List<UserResponse>>.Handle(Query request, CancellationToken cancellationToken)
        {
            var userResponse = await _context
                .Users
                .Select(user => new UserResponse
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    CreatedOnUtc = user.CreatedDate,

                    Pets = user.OwnedPets.Select(pet => new PetResponse
                    {
                        Name = pet.Name,
                        Type = pet.Type,
                        BirthDate = pet.BirthDate,
                        Color = pet.Color,
                        Gender = pet.Gender,

                    }).ToList()
                })
                .ToListAsync(cancellationToken);
            return userResponse;
        }
    }
}

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/users", async (ISender sender) =>
        {
            var query = new GetUsers.Query();

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
