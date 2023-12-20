﻿using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;

namespace PetManagement.Features.Trainings;

public class GetTraining
{
    public class Query : IRequest<TrainingResponse>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, TrainingResponse>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<TrainingResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var trainingResponse = await _context
                .Trainings
                .Where(training => training.Id == request.Id)
                .Select(training => new TrainingResponse
                {
                    Name = training.Name,

                })
                .FirstOrDefaultAsync(cancellationToken);

            return trainingResponse;
        }
    }
}

public class GetTrainingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/training/{id}", async (int id, ISender sender) =>
        {
            var query = new GetTraining.Query { Id = id };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
