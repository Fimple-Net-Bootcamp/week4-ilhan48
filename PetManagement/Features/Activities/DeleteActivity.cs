using Carter;
using MediatR;
using PetManagement.Database;

namespace PetManagement.Features.Activities;

public static class DeleteActivity
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
            var activity = await _context.Activities.FindAsync(request.Id);

            if (activity != null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
public class DeleteActivityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/activities/{id}", async (int id, ISender sender) =>
        {
            var command = new DeleteActivity.Command { Id = id };

            await sender.Send(command);

            return Results.NoContent();
        });
    }
}
