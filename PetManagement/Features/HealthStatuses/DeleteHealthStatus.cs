using Carter;
using MediatR;
using PetManagement.Database;
using PetManagement.Database.Context;

namespace PetManagement.Features.HealthStatuses;

public static class DeleteHealthStatus
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
            var healthStatus = await _context.HealthStatuses.FindAsync(request.Id);

            if (healthStatus != null)
            {
                _context.HealthStatuses.Remove(healthStatus);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
public class DeleteHealthStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/healthstatuses/{id}", async (int id, ISender sender) =>
        {
            var command = new DeleteHealthStatus.Command { Id = id };

            await sender.Send(command);

            return Results.NoContent();
        });
    }
}