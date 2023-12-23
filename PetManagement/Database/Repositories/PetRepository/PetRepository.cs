using PetManagement.Database.Context;
using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.PetRepository;

public class PetRepository : EfRepositoryBase<Pet, int, PetManagementDbContext>, IPetRepository
{
    public PetRepository(PetManagementDbContext context) : base(context) { }


}