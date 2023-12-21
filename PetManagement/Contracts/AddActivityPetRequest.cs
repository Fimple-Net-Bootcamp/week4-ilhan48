using PetManagement.Entities;

namespace PetManagement.Contracts;

public class AddActivityPetRequest
{
    public int PetId { get; set; }
    public int ActivityId { get; set; }
}
