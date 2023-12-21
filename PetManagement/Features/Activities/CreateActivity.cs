using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;

namespace PetManagement.Features.Activities;

public static class CreateActivity
{
    public class Command : IRequest<int>
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DifficultyLevel { get; set; }
    }

    public class CreateActivityCommandValidator : AbstractValidator<Command>
    {
        public CreateActivityCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required!")
                .MinimumLength(3).WithMessage("Name is too short!")
                .MaximumLength(25).WithMessage("Name is too long!");

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
            var pet = await _context.Pets
            .Include(p => p.Activities)
            .FirstOrDefaultAsync(p => p.Id == request.PetId);

            var entity = new Activity
            {
                Name = request.Name,
                Description = request.Description,
                DifficultyLevel = request.DifficultyLevel,
                CreatedDate = DateTime.UtcNow,
            };

            pet.Activities.Add(entity);

            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
public class CreateActivityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/activities", async (CreateActivityRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateActivity.Command>();
            var id = await sender.Send(command);

            return Results.Created($"/activities/{id}", request);
        });
    }
}
