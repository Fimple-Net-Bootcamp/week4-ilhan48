using Carter;
using MediatR;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.FoodRepository;

namespace PetManagement.Features.Foods;

public static class DeleteFood
{
    public class Command : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IFoodRepository  _foodRepository;

        public Handler(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var food = _foodRepository.Get(f => f.Id == request.Id);

            if (food != null)
            {
                _foodRepository.Delete(food);
            }

            return Unit.Value;
        }
    }
}
public class DeleteFoodEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/foods/{id}", async (int id, ISender sender) =>
        {
            var command = new DeleteFood.Command { Id = id };

            await sender.Send(command);

            return Results.NoContent();
        });
    }
}
