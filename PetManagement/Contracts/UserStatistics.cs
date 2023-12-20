namespace PetManagement.Contracts;

public class UserStatistics
{
    public Guid UserId { get; set; }
    public PetStatistics PetStatistics { get; set; }
}