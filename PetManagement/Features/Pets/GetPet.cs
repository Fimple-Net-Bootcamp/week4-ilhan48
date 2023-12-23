using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.PetRepository;
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
        private readonly IPetRepository _petRepository;

        public Handler(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<PetResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var pet = _petRepository.Get(p => p.Id == request.Id);
            var petResponse = new PetResponse
            {
                Name = pet.Name,
                Gender = pet.Gender,
                Type = pet.Type,
                Color = pet.Color,
                BirthDate = pet.BirthDate,
                OwnerId = pet.OwnerId,
            };

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
