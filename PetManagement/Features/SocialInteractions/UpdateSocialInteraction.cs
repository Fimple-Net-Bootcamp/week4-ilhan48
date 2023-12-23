using Carter;
using MediatR;
using PetManagement.Database.Context;

namespace PetManagement.Features.SocialInteractions;

public class UpdateSocialInteraction
{
    public class UpdateSocialInteractionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    internal sealed class Handler : IRequestHandler<UpdateSocialInteractionCommand, Unit>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateSocialInteractionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var socialInteraction = await _context.SocialInteractions.FindAsync(request.Id);

                if (socialInteraction != null)
                {
                    socialInteraction.Name = request.Name;
                    socialInteraction.StartDate = request.StartDate;
                    socialInteraction.EndDate = request.EndDate;
                    socialInteraction.UpdatedDate = DateTime.UtcNow;

                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateSocialInteraction.Handler: {ex.Message}");
                throw;
            }
        }
    }
}

public class UpdateSocialInteractionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/socialinteractions/{id}", async (int id, UpdateSocialInteraction.UpdateSocialInteractionCommand request, ISender sender) =>
        {
            request.Id = id;

            await sender.Send(request);

            return Results.NoContent();
        });
    }
}

