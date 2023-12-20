namespace PetManagement.Entities;

public class User : BaseEntity<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public bool Status { get; set; }

    public virtual ICollection<Pet> OwnedPets { get; set; }

    public User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        UserName = string.Empty;
        OwnedPets = new HashSet<Pet>();
    }

    public User(
        string firstName,
        string lastName,
        string email,
        string userName,
        bool status
    )
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
        Status = status;
    }
    public User(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string userName,
        bool status
    )
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
        Status = status;
    }
}

