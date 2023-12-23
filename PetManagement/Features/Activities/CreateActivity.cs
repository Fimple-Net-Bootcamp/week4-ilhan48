using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.ActivityRepository;
using PetManagement.Entities;
using PetManagement.Shared;
using static PetManagement.Shared.ExceptionMiddleware;

namespace PetManagement.Features.Activities;

public static class CreateActivity
{

    public class Command : IRequest<CommandResult<int>>
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

    internal sealed class Handler : IRequestHandler<Command, CommandResult<int>>
    {
        private readonly PetManagementDbContext _context;
        private readonly IActivityRepository _activityRepository;
        private readonly IValidator<Command> _validator;

        public Handler(PetManagementDbContext context, IActivityRepository activityRepository, IValidator<Command> validator)
        {
            _context = context;
            _activityRepository = activityRepository;
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

            _activityRepository.Add(entity);
            return new CommandResult<int> { Id = entity.Id };
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
            var result = await sender.Send(command);

            if (result.Errors != null && result.Errors.Any())
            {
                throw new ExceptionResponse(result.Errors, StatusCodes.Status400BadRequest);
            }

            return Results.Created($"/activities/{result.Id}", request);

        });
    }
}
