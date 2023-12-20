using Carter;
using MediatR;
using PetManagement.Database;

namespace PetManagement.Features.Users;

public static class DeleteUser
{
    public class Command : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handle : IRequestHandler<Command, Guid>
    {
        private readonly PetManagementDbContext _context;

        public Handle(PetManagementDbContext context)
        {
            _context = context;
        }

        async Task<Guid> IRequestHandler<Command, Guid>.Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.Id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync(cancellationToken);
                return user.Id;
            }

            return Guid.Empty;
        }
    }
}

public class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/users/{id}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteUser.Command { Id = id };

            var deletedUserId = await sender.Send(command);

            if (deletedUserId != Guid.Empty)
            {
                return Results.Ok(deletedUserId);
            }

            return Results.NotFound();
        });
    }
}
