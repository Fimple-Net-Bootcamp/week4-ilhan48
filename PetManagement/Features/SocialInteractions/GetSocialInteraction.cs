using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;

namespace PetManagement.Features.SocialInteractions;

public static class GetSocialInteraction
{
    public class Query : IRequest<SocialInteractionResponse>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, SocialInteractionResponse>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<SocialInteractionResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var socialInteractionResponse = await _context
                .SocialInteractions
                .Where(socialInteraction => socialInteraction.Id == request.Id)
                .Select(socialInteraction => new SocialInteractionResponse
                {
                    Name = socialInteraction.Name,

                })
                .FirstOrDefaultAsync(cancellationToken);

            return socialInteractionResponse;
        }
    }
}

public class GetSocialInteractionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/socialinteractions/{id}", async (int id, ISender sender) =>
        {
            var query = new GetSocialInteraction.Query { Id = id };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
