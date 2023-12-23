using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.PetRepository;
using PetManagement.Entities;

namespace PetManagement.Features.Pets;

public static class GetPets
{
    public class Query : IRequest<List<PetResponse>>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, List<PetResponse>>
    {
        private readonly IPetRepository _petRepository;

        public Handler(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<List<PetResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<Pet> pets = new List<Pet>();

            foreach (var pet in _petRepository.GetAll())
            {
                pets.Add(pet);
            }

            List<PetResponse> responses = new List<PetResponse>();
            for (int i = 0; i < pets.Count; i++)
            {
                var petResponse = new PetResponse
                {
                    Name = pets[i].Name,
                    Color = pets[i].Color,
                    BirthDate = pets[i].BirthDate,
                    Gender = pets[i].Gender,
                    OwnerId = pets[i].OwnerId,
                    Type = pets[i].Type,

                };
                responses.Add(petResponse);
            }

            return responses;


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