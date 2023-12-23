using Carter;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Database.Repositories.FoodRepository;

namespace PetManagement.Features.Foods;

public static class GetFood
{
    public class Query : IRequest<FoodResponse>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, FoodResponse>
    {
        private readonly IFoodRepository _foodRepository;

        public Handler(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }


        public async Task<FoodResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var food = _foodRepository.Get(p => p.Id == request.Id);

            var foodResponse = new FoodResponse
                {
                    Name = food.Name,

                };

            return foodResponse;
        }
    }
}

public class GetFoodEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/foods/{id}", async (int id, ISender sender) =>
        {
            var query = new GetFood.Query { Id = id };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
