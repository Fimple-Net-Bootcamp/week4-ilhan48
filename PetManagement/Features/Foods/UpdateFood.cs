using Carter;
using MediatR;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.FoodRepository;
using PetManagement.Features.Pets;

namespace PetManagement.Features.Foods;

public class UpdateFood
{
    public class UpdateFoodCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal sealed class Handler : IRequestHandler<UpdateFoodCommand, Unit>
    {
        private readonly IFoodRepository _foodRepository;

        public Handler(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        public async Task<Unit> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var food = _foodRepository.Get(f => f.Id == request.Id);

                if (food != null)
                {
                    food.Name = request.Name;
                    food.UpdatedDate = DateTime.UtcNow;

                    _foodRepository.Update(food);
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateFood.Handler: {ex.Message}");
                throw;
            }
        }
    }
}

public class UpdateFoodEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/foods/{id}", async (int id, UpdateFood.UpdateFoodCommand request, ISender sender) =>
        {
            request.Id = id;

            await sender.Send(request);

            return Results.NoContent();
        });
    }
}
