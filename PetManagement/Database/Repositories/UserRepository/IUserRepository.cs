using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.UserRepository;

public interface IUserRepository : IAsyncRepository<User, Guid>, IRepository<User, Guid> { }
