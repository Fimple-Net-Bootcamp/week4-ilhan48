using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;

namespace PetManagement.Features.Pets;

public static class GetPets
{
    public class Query : IRequest<List<PetResponse>>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, List<PetResponse>>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<PetResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var petResponse = await _context
                .Pets
                .Select(pet => new PetResponse
                {
                    Name = pet.Name,
                    Gender = pet.Gender,
                    Color = pet.Color,
                    Type = pet.Type,
                    BirthDate = pet.BirthDate,
                    OwnerId = pet.OwnerId,
                })
                .ToListAsync(cancellationToken);

            return petResponse;
            
        }
    }
}

public class GetPetsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/pets", async (ISender sender) =>
        {
            var query = new GetPets.Query();

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}