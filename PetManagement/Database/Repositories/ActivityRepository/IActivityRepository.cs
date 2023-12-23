using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.ActivityRepository;

public interface IActivityRepository : IAsyncRepository<Activity, int>, IRepository<Activity, int>
{
}
