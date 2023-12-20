using Carter;
using Mapster;
using MediatR;
using PetManagement.Database;

namespace PetManagement.Features.Users;

public static class UpdateUser
{
    public class Command : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
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
            var user = await _context.Users.FindAsync(request.Id);

            if (user != null)
            {
                user = request.Adapt(user);

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}

public class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/users/{id}", async (Guid id, UpdateUser.Command request, ISender sender) =>
        {
            request.Id = id;

            await sender.Send(request);

            return Results.NoContent();
        });
    }
}
