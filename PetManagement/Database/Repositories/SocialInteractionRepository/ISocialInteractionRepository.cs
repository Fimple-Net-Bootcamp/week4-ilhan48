using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.SocialInteractionRepository;

public interface ISocialInteractionRepository : IAsyncRepository<SocialInteraction, int>, IRepository<SocialInteraction, int>
{
}
