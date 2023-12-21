using PetManagement.Entities;

namespace PetManagement.Contracts;

public class PetStatistics
{
    public int PetId { get; set; }
    public ICollection<FoodResponse>? Foods { get; set; }
    public ICollection<TrainingResponse>? Trainings { get; set; }
    public ICollection<ActivityResponse>? Activities { get; set; }

}
