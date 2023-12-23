using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.FoodRepository;

public interface IFoodRepository : IAsyncRepository<Food, int>, IRepository<Food, int> { }
