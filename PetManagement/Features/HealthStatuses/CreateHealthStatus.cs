﻿using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Database.Context;
using PetManagement.Entities;
using PetManagement.Shared;
using static PetManagement.Shared.ExceptionMiddleware;

namespace PetManagement.Features.HealthStatuses;

public static class CreateHealthStatus
{
    

    public class Command : IRequest<CommandResult<int>>
    {
        public DateTime ExaminationDate { get; set; }
        public bool VaccinationStatus { get; set; }
        public string DiseaseStatus { get; set; }
        public string TreatmentInfo { get; set; }
        public string Notes { get; set; }
        public int PetId { get; set; }
    }

    public class CreateHealthStatusCommandValidator : AbstractValidator<Command>
    {
        public CreateHealthStatusCommandValidator()
        {
            RuleFor(x => x.DiseaseStatus)
                .NotEmpty().WithMessage("DiseaseStatus is required!")
                .MinimumLength(3).WithMessage("DiseaseStatus is too short!")
                .MaximumLength(25).WithMessage("DiseaseStatus is too long!");

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

            var entity = new HealthStatus
            {
                DiseaseStatus = request.DiseaseStatus,
                ExaminationDate = request.ExaminationDate,
                VaccinationStatus = request.VaccinationStatus,
                TreatmentInfo = request.TreatmentInfo,
                Notes = request.Notes,
                PetId = request.PetId,
                CreatedDate = DateTime.UtcNow,
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            return new CommandResult<int> { Id = entity.Id };
        }
    }
}
public class CreateHealthStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/healthstatuses", async (CreateHealthStatusRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateHealthStatus.Command>();
            var result = await sender.Send(command);

            if (result.Errors != null && result.Errors.Any())
            {
                throw new ExceptionResponse(result.Errors, StatusCodes.Status400BadRequest);
            }

            return Results.Created($"/healthstatuses/{result.Id}", request);
        });
    }
}
