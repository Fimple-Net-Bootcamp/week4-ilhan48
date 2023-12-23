using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;

namespace PetManagement.Features.SocialInteractions;

public static class GetSocialInteractions
{
    public class Query : IRequest<List<SocialInteractionResponse>>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, List<SocialInteractionResponse>>
    {
        private readonly PetManagementDbContext _context;

        public Handler(PetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<SocialInteractionResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var socialInteractionResponse = await _context
                .SocialInteractions
                .Select(socialInteractionResponse => new SocialInteractionResponse
                {
                    Name = socialInteractionResponse.Name,
                    StartDate = socialInteractionResponse.StartDate,
                    EndDate = socialInteractionResponse.EndDate,

                })
                .ToListAsync(cancellationToken);

            return socialInteractionResponse;

        }
    }
}

public class GetSocialInteractionsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/socialInteractions", async (ISender sender) =>
        {
            var query = new GetSocialInteraction.Query();

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}