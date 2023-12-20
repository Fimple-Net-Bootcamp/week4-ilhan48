using Carter;
using MediatR;
using PetManagement.Database;
using PetManagement.Features.Foods;

namespace PetManagement.Features.Trainings;

public class UpdateTraining
{
    public class UpdateTrainingCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal sealed class Handler : IRequestHandler<UpdateTrainingCommand, Unit>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTrainingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var training = await _context.Trainings.FindAsync(request.Id);

                if (training != null)
                {
                    training.Name = request.Name;
                    training.UpdatedDate = DateTime.UtcNow;

                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateTraining.Handler: {ex.Message}");
                throw;
            }
        }
    }
}

public class UpdateTrainingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/trainings/{id}", async (int id, UpdateTraining.UpdateTrainingCommand request, ISender sender) =>
        {
            request.Id = id;

            await sender.Send(request);

            return Results.NoContent();
        });
    }
}
