using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.TrainingRepository;

public interface ITrainingRepository : IAsyncRepository<Training, int>, IRepository<Training, int>
{
}
