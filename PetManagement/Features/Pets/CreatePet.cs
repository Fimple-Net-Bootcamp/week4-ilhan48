using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;
using PetManagement.Features.Users;

namespace PetManagement.Features.Pets;

public static class CreatePet
{
    public class Command : IRequest<int>
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime BirthDate { get; set; }
        public string Color { get; set; }
        public string Gender { get; set; }
        public Guid? OwnerId { get; set; }
    }
    public class CreatePetCommandValidator : AbstractValidator<Command>
    {
        public CreatePetCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("pet name is required!")
                .MinimumLength(3).WithMessage("name is too short!")
                .MaximumLength(25).WithMessage("name is too long!");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("gender is required!")
                .MinimumLength(2).WithMessage("gender is too short!")
                .MaximumLength(50).WithMessage("gender is too long!");
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

            var entity = new Pet
            {
                Name = request.Name,
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                Color = request.Color,
                Type = request.Type,
                OwnerId = request.OwnerId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
public class CreatePetEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/pets", async (CreatePetRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreatePet.Command>();
            var petId = await sender.Send(command);

            return Results.Created($"/pets/{petId}", request);
        });
    }
}
