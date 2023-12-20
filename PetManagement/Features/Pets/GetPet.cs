using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Features.Users;

namespace PetManagement.Features.Pets;

public static class GetPet
{
    public class Query : IRequest<PetResponse>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, PetResponse>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<PetResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var petResponse = await _context
                .Pets
                .Where(pet => pet.Id == request.Id)
                .Select(pet => new PetResponse
                {
                    Name = pet.Name,
                    Gender = pet.Gender,
                    Type = pet.Type,
                    Color = pet.Color,
                    BirthDate = pet.BirthDate,
                    OwnerId = pet.OwnerId,
                })
                .FirstOrDefaultAsync(cancellationToken);

            return petResponse;
        }
    }
}

public class GetUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/pets/{id}", async (int id, ISender sender) =>
        {
            var query = new GetPet.Query { Id = id };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
