using Carter;
using MediatR;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.PetRepository;
using PetManagement.Features.Users;

namespace PetManagement.Features.Pets;

public static class DeletePet
{
    public class Command : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Unit> 
    {
        private readonly IPetRepository _petRepository;

        public Handler(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var pet = _petRepository.Get(p => p.Id == request.Id);

            if (pet != null)
            {
                _petRepository.Delete(pet);
            }

            return Unit.Value;
        }
    }
}
public class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/pets/{id}", async (int id, ISender sender) =>
        {
            var command = new DeletePet.Command { Id = id };

            await sender.Send(command);

            return Results.NoContent();
        });
    }
}
