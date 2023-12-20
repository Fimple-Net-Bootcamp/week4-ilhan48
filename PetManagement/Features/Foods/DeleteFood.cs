using Carter;
using MediatR;
using PetManagement.Database;

namespace PetManagement.Features.Foods;

public static class DeleteFood
{
    public class Command : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Unit>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var food = await _context.Foods.FindAsync(request.Id);

            if (food != null)
            {
                _context.Foods.Remove(food);
                await _context.SaveChangesAsync(cancellationToken);
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
