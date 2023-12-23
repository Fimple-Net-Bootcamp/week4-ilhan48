using PetManagement.Database.Context;
using PetManagement.Database.Repositories.Core;
using PetManagement.Entities;

namespace PetManagement.Database.Repositories.TrainingRepository;

public class TrainingRepository : EfRepositoryBase<Training, int, PetManagementDbContext>, ITrainingRepository
{
    public TrainingRepository(PetManagementDbContext context) : base(context) { }
}
