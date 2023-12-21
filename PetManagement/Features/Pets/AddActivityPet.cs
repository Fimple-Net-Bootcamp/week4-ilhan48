using Carter;
using Mapster;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;

namespace PetManagement.Features.Pets;

public static class AddActivityPet
{
    public class Command : IRequest<int>
    {
        public int PetId { get; set; }
        public int ActivityId { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, int>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var pet = await _context.Pets.FindAsync(request.PetId);

            var activity = await _context.Activities.FindAsync(request.ActivityId);

            if (pet != null && activity != null)
            {
                pet.Activities.Add(activity);

                await _context.SaveChangesAsync();
            }

            return pet.Id;
        }
    }
}

public class AddActivityPetEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/pets/{petId}/activities/", async (AddActivityPetRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreatePet.Command>();
            var petId = await sender.Send(command);
            await sender.Send(command);
            return Results.NoContent();
        });

    }
}
