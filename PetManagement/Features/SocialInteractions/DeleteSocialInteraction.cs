using Carter;
using MediatR;
using PetManagement.Database.Context;

namespace PetManagement.Features.SocialInteractions;

public class DeleteSocialInteraction
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
            var socialInteraction = await _context.SocialInteractions.FindAsync(request.Id);

            if (socialInteraction != null)
            {
                _context.SocialInteractions.Remove(socialInteraction);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
public class DeleteSocialInteractionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/socialinteractions/{id}", async (int id, ISender sender) =>
        {
            var command = new DeleteSocialInteraction.Command { Id = id };

            await sender.Send(command);

            return Results.NoContent();
        });
    }
}
