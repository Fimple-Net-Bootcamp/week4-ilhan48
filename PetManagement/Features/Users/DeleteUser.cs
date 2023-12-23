using Carter;
using MediatR;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.UserRepository;

namespace PetManagement.Features.Users;

public static class DeleteUser
{
    public class Command : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handle : IRequestHandler<Command, Unit>
    {
        private readonly IUserRepository _userRepository;

        public Handle(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        async Task<Unit> IRequestHandler<Command, Unit>.Handle(Command request, CancellationToken cancellationToken)
        {
            var user = _userRepository.Get(u => u.Id == request.Id);

            if (user != null)
            {
                _userRepository.Delete(user);
            }

            return Unit.Value;
        }
    }
}

public class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/users/{id}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteUser.Command { Id = id };

            await sender.Send(command);

            return Results.NoContent();
        });
    }
}
