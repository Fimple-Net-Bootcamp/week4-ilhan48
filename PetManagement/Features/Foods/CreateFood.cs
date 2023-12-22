using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;
using PetManagement.Shared;
using static PetManagement.Shared.ExceptionMiddleware;

namespace PetManagement.Features.Foods;

public static class CreateFood
{
    

    public class Command : IRequest<CommandResult<int>>
    {
        public string Name { get; set; }
        public int PetId { get; set; }
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

    internal sealed class Handler : IRequestHandler<Command, CommandResult<int>>
    {
        private readonly PetManagementDbContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(PetManagementDbContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<CommandResult<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                return new CommandResult<int> { Errors = errors };
            }
            var pet = await _context.Pets
            .Include(p => p.Foods) 
            .FirstOrDefaultAsync(p => p.Id == request.PetId);


            var entity = new Food
            {
                Name = request.Name,
                CreatedDate = DateTime.UtcNow,
            };

            pet.Foods.Add(entity);
            

            _context.Add(entity);
            await _context.SaveChangesAsync();
            return new CommandResult<int> { Id = entity.Id };
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
            var result = await sender.Send(command);

            if (result.Errors != null && result.Errors.Any())
            {
                throw new ExceptionResponse(result.Errors, StatusCodes.Status400BadRequest);
            }

            return Results.Created($"/foods/{result.Id}", request);
        });
    }
}

