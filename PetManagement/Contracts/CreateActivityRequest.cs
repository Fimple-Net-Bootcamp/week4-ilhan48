namespace PetManagement.Contracts;

public class CreateActivityRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string DifficultyLevel { get; set; }
}
