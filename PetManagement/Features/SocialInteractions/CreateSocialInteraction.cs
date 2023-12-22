using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;
using PetManagement.Shared;

namespace PetManagement.Features.SocialInteractions;

public static class CreateSocialInteraction
{

    public class Command : IRequest<CommandResult<int>>
    {
        public string Name { get; set; }
        public int PetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class CreateSocialInteractionCommandValidator : AbstractValidator<Command>
    {
        public CreateSocialInteractionCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Social Interaction name is required!")
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
            .Include(p => p.SocialInteractions)
            .FirstOrDefaultAsync(p => p.Id == request.PetId);

            var entity = new SocialInteraction
            {
                Name = request.Name,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreatedDate = DateTime.UtcNow,
            };

            pet.SocialInteractions.Add(entity);

            _context.Add(entity);
            await _context.SaveChangesAsync();
            return new CommandResult<int> { Id = entity.Id };
        }
    }
}

public class CreateSocialInteractionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/socialinteractions", async (CreateSocialInteractionRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateSocialInteraction.Command>();

            var result = await sender.Send(command);

            if (result.Errors != null && result.Errors.Any())
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Created($"/socialinteractions/{result.Id}", request);
        });
    }
}


