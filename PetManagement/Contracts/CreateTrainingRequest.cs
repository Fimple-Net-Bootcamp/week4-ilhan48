namespace PetManagement.Contracts;

public class CreateTrainingRequest
{
    public int PetId { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
}
