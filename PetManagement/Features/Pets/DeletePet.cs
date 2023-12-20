using Carter;
using MediatR;
using PetManagement.Database;
using PetManagement.Features.Users;

namespace PetManagement.Features.Pets;

public static class DeletePet
{
    public class Command : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    internal sealed class Handle : IRequestHandler<Command, Unit> 
    {
        private readonly PetManagementDbContext _context;

        public Handle(PetManagementDbContext context)
        {
            _context = context;
        }

        async Task<Unit> IRequestHandler<Command, Unit>.Handle(Command request, CancellationToken cancellationToken)
        {
            var pet = await _context.Pets.FindAsync(request.Id);

            if (pet != null)
            {
                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
public class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/pets/{id}", async (int id, ISender sender) =>
        {
            var command = new DeletePet.Command { Id = id };

            await sender.Send(command);

            return Results.NoContent();
        });
    }
}
