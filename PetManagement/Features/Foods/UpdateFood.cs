using Carter;
using MediatR;
using PetManagement.Database;
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
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var food = await _context.Pets.FindAsync(request.Id);

                if (food != null)
                {
                    food.Name = request.Name;
                    food.UpdatedDate = DateTime.UtcNow;

                    await _context.SaveChangesAsync(cancellationToken);
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
