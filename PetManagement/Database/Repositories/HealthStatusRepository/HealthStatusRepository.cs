using PetManagement.Database.Context;
using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.HealthStatusRepository;

public class HealthStatusRepository : EfRepositoryBase<HealthStatus, int, PetManagementDbContext>,
                                      IHealthStatusRepository
{
    public HealthStatusRepository(PetManagementDbContext context) : base(context) { }
}
