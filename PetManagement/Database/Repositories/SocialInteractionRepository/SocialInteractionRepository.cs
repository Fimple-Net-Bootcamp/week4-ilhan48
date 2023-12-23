using PetManagement.Database.Context;
using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.SocialInteractionRepository;

public class SocialInteractionRepository : EfRepositoryBase<SocialInteraction, int, PetManagementDbContext>, ISocialInteractionRepository
{
    public SocialInteractionRepository(PetManagementDbContext context) : base(context) { }
}
