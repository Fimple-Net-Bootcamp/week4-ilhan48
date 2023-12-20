using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;

namespace PetManagement.Features.Foods;

public static class CreateFood
{
    public class Command : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreateFoodCommandValidator : AbstractValidator<Command>
    {
        public CreateFoodCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("food name is required!")
                .MinimumLength(3).WithMessage("name is too short!")
                .MaximumLength(25).WithMessage("name is too long!");

        }
    }

    internal sealed class Handler : IRequestHandler<Command, int>
    {
        private readonly PetManagementDbContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(PetManagementDbContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {

            }

            var entity = new Food
            {
                Name = request.Name,
                CreatedDate = DateTime.UtcNow,
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}

public class CreateFoodEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/foods", async (CreateFoodRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateFood.Command>();

            var userId = await sender.Send(command);
            return Results.Created($"/foods/{request.Name}", request);
        });
    }
}

