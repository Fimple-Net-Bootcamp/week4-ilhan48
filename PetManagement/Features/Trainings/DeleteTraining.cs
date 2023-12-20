using Carter;
using MediatR;
using PetManagement.Database;

namespace PetManagement.Features.Trainings;

public static class DeleteTraining
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
            var training = await _context.Trainings.FindAsync(request.Id);

            if (training != null)
            {
                _context.Trainings.Remove(training);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
public class DeleteTrainingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/trainings/{id}", async (int id, ISender sender) =>
        {
            var command = new DeleteTraining.Command { Id = id };

            await sender.Send(command);

            return Results.NoContent();
        });
    }
}
