using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;

namespace PetManagement.Features.Foods;

public static class GetFood
{
    public class Query : IRequest<FoodResponse>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, FoodResponse>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<FoodResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var foodResponse = await _context
                .Foods
                .Where(food => food.Id == request.Id)
                .Select(food => new FoodResponse
                {
                    Name = food.Name,
                    
                })
                .FirstOrDefaultAsync(cancellationToken);

            return foodResponse;
        }
    }
}

public class GetUserEndpoint : ICarterModule
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
