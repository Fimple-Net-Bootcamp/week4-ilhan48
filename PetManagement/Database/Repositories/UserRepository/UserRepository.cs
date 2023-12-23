using PetManagement.Database.Context;
using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.UserRepository;
public class UserRepository : EfRepositoryBase<User, Guid, PetManagementDbContext>, IUserRepository
{
    public UserRepository(PetManagementDbContext context) : base(context) { }


}
