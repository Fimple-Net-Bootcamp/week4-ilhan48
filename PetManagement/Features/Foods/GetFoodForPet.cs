using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;

namespace PetManagement.Features.Foods;

public static class GetFoodForPet
{
    public class Query : IRequest<List<FoodResponse>>
    {
        public int PetId { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, List<FoodResponse>>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoodResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var pet = await _context.Pets
            .Include(p => p.Foods)
            .FirstOrDefaultAsync(p => p.Id == request.PetId);

            List<Food> foods = new List<Food>();

            foreach(var food in pet.Foods)
            {
                foods.Add(food);
            }

            List<FoodResponse> responses = new List<FoodResponse>();
            for (int i = 0; i < foods.Count; i++)
            {
                var foodResponse = new FoodResponse
                {
                    Name = foods[i].Name,

                };
                responses.Add(foodResponse);
            }

            return responses;
        }
    }
}

public class GetFoodForPetEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/{pet-id}/foods", async (int petId, ISender sender) =>
        {
            var query = new GetFoodForPet.Query { PetId = petId };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
