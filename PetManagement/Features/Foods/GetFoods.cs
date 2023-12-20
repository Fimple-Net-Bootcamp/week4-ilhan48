using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;

namespace PetManagement.Features.Foods;

public static class GetFoods
{
    public class Query : IRequest<List<FoodResponse>>
    {

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
            var foodResponse = await _context
                .Foods
                .Select(food => new FoodResponse
                {
                    Name = food.Name,
                    
                })
                .ToListAsync(cancellationToken);

            return foodResponse;

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