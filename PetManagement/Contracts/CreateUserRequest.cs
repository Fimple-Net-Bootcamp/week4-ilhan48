namespace PetManagement.Contracts;

public class CreateUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
}

public class CreateUserCommandResponse
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
}