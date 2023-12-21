namespace PetManagement.Contracts;

public class CreateActivityRequest
{
    public int PetId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string DifficultyLevel { get; set; }
}
