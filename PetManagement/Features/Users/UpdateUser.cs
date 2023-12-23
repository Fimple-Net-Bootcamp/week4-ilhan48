using Carter;
using Mapster;
using MediatR;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.UserRepository;

namespace PetManagement.Features.Users;

public static class UpdateUser
{
    public class Command : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUserRepository _userRepository;

        public Handler (IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = _userRepository.Get(u => u.Id == request.Id);

            if (user != null)
            {
                user = request.Adapt(user);

                _userRepository.Update(user);
            }

            return Unit.Value;
        }
    }
}

public class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/users/{id}", async (Guid id, UpdateUser.Command request, ISender sender) =>
        {
            request.Id = id;

            await sender.Send(request);

            return Results.NoContent();
        });
    }
}
