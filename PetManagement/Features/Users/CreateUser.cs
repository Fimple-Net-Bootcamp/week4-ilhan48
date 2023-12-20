using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;

namespace PetManagement.Features.Users;

public static class CreateUser
{
    public class Command : IRequest<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }

    public class CreateCustomerCommandValidator : AbstractValidator<Command>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("your name is required!")
                .MinimumLength(3).WithMessage("name is too short!")
                .MaximumLength(25).WithMessage("name is too long!");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("email address is required!")
                .EmailAddress().WithMessage("the format of your email address is wrong!");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("a username is required!")
                .MinimumLength(3).WithMessage("username is too short!")
                .MaximumLength(50).WithMessage("username is too long!");
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Guid>
    {
        private readonly PetManagementDbContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(PetManagementDbContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                
            }

            var passwordSalt = Guid.NewGuid().ToByteArray();
            var entity = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                CreatedDate = DateTime.UtcNow,
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}

public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/users", async (CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUser.Command>();

            var userId = await sender.Send(command);
            return Results.Created($"/users/{request.Email}", request);
        });
    }
}
