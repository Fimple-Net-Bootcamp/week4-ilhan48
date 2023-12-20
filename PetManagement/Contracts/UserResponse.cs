namespace PetManagement.Contracts;

public class UserResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}
