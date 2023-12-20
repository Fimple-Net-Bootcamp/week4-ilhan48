namespace PetManagement.Contracts;

public class PetResponse
{
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime BirthDate { get; set; }
    public string Color { get; set; }
    public string Gender { get; set; }
    public Guid? OwnerId { get; set; }
}
