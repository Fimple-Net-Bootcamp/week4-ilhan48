using Carter;
using MediatR;
using PetManagement.Database;

namespace PetManagement.Features.Pets;

public static class UpdatePet
{
    public class UpdatePetCommand : IRequest<Unit>
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime BirthDate { get; set; }
        public string Color { get; set; }
        public string Gender { get; set; }
        public Guid? OwnerId { get; set; }
    }

    internal sealed class Handler : IRequestHandler<UpdatePetCommand, Unit>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var pet = await _context.Pets.FindAsync(request.PetId);

                if (pet != null)
                {
                    pet.Name = request.Name;
                    pet.Type = request.Type;
                    pet.BirthDate = request.BirthDate;
                    pet.Color = request.Color;
                    pet.Gender = request.Gender;
                    pet.OwnerId = request.OwnerId;

                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdatePet.Handler: {ex.Message}");
                throw;
            }
        }
    }
}

public class UpdatePetEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/pets/{id}", async (int id, UpdatePet.UpdatePetCommand request, ISender sender) =>
        {
            request.PetId = id;

            await sender.Send(request);

            return Results.NoContent();
        });
    }
}