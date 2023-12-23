using PetManagement.Database.Context;
using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.ActivityRepository;

public class ActivityRepository : EfRepositoryBase<Activity, int, PetManagementDbContext>, IActivityRepository
{
    public ActivityRepository(PetManagementDbContext context) : base(context) { }
}
