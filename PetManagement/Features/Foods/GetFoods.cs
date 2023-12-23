using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.FoodRepository;
using PetManagement.Entities;

namespace PetManagement.Features.Foods;

public static class GetFoods
{
    public class Query : IRequest<List<FoodResponse>>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, List<FoodResponse>>
    {
        private readonly IFoodRepository _foodRepository;

        public Handler(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }


        public async Task<List<FoodResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<Food> foods = new List<Food>();

            foreach (var food in _foodRepository.GetAll())
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

public class GetFoodsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/foods", async (ISender sender) =>
        {
            var query = new GetFoods.Query();

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}