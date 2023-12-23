using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.HealthStatusRepository;

public interface IHealthStatusRepository : IAsyncRepository<HealthStatus, int>,
                                           IRepository<HealthStatus, int>
{
}
