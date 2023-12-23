using PetManagement.Database.Context;
using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.FoodRepository;

public class FoodRepository : EfRepositoryBase<Food, int, PetManagementDbContext>, IFoodRepository
{
    public FoodRepository(PetManagementDbContext context) : base(context) { }
}
