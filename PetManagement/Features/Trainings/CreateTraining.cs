using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;

namespace PetManagement.Features.Trainings;

public static class CreateTraining
{
    public class Command : IRequest<int>
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class CreateTrainingCommandValidator : AbstractValidator<Command>
    {
        public CreateTrainingCommandValidator()
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

            var entity = new Training
            {
                Name = request.Name,
                Date = request.Date,
                CreatedDate = DateTime.UtcNow,
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}

public class CreateTrainingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/trainings", async (CreateTrainingRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateTraining.Command>();

            var userId = await sender.Send(command);
            return Results.Created($"/trainings/{request.Name}", request);
        });
    }
}


