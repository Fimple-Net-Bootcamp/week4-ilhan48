using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database;
using PetManagement.Entities;

namespace PetManagement.Features.SocialInteractions;

public static class GetSocialInteractionForPet
{
    public class Query : IRequest<List<SocialInteractionResponse>>
    {
        public int PetId { get; set; }
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
            var pet = await _context.Pets
            .Include(p => p.SocialInteractions)
            .FirstOrDefaultAsync(p => p.Id == request.PetId);

            List<SocialInteraction> socialInteractions = new List<SocialInteraction>();

            foreach (var socialInteraction in pet.SocialInteractions)
            {
                socialInteractions.Add(socialInteraction);
            }

            List<SocialInteractionResponse> responses = new List<SocialInteractionResponse>();
            for (int i = 0; i < socialInteractions.Count; i++)
            {
                var socialInteractionsResponse = new SocialInteractionResponse
                {
                    Name = socialInteractions[i].Name,
                    StartDate = socialInteractions[i].StartDate,
                    EndDate = socialInteractions[i].EndDate,

                };
                responses.Add(socialInteractionsResponse);
            }

            return responses;
        }
    }
}

public class GetSocialInteractionForPetEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/{pet-id}/socialinteractions", async (int petId, ISender sender) =>
        {
            var query = new GetSocialInteractionForPet.Query { PetId = petId };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
