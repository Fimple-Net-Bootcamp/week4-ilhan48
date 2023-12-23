using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.PetRepository;
public interface IPetRepository : IAsyncRepository<Pet, int>, IRepository<Pet, int> { }
