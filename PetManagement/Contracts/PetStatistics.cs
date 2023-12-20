namespace PetManagement.Contracts;

public class PetStatistics
{
    public int PetId { get; set; }
    public ActivityResponse ActivityStatistics { get; set; }
    public HealthStatusResponse HealthStatusStatistics { get; set; }
    public FoodResponse FoodStatistics { get; set; }
}
