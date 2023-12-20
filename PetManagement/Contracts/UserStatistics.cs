namespace PetManagement.Contracts;

public class UserStatistics
{
    public Guid UserId { get; set; }
    public List<PetResponse> OwnedPets { get; set; }
}