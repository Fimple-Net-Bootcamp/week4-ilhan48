using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;
using PetManagement.Shared;
using static PetManagement.Shared.ExceptionMiddleware;

namespace PetManagement.Features.Users;

public static class CreateUser
{
    
    public class Command : IRequest<CommandResult<Guid>>
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

    internal sealed class Handler : IRequestHandler<Command, CommandResult<Guid>>
    {
        private readonly PetManagementDbContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(PetManagementDbContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<CommandResult<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                return new CommandResult<Guid> { Errors = errors };
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
            return new CommandResult<Guid> { Id = entity.Id };
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

            var result = await sender.Send(command);

            if (result.Errors != null && result.Errors.Any())
            {
                throw new ExceptionResponse(result.Errors, StatusCodes.Status400BadRequest);
            }

            return Results.Created($"/users/{request.Email}", result.Id);
        });
    }
}
