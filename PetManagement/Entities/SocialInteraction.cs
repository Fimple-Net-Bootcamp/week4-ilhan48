namespace PetManagement.Entities;

public class SocialInteraction : BaseEntity<int>
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public virtual ICollection<Pet> Pets { get; set; }

    public SocialInteraction()
    {
        Name = string.Empty;
        StartDate = DateTime.UtcNow;
        EndDate = DateTime.UtcNow;
        Pets = new List<Pet>();
    }
    public SocialInteraction(string name, DateTime startDate, DateTime endDate, ICollection<Pet> pets)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        Pets = pets ?? new List<Pet>();
    }
    public SocialInteraction(int id, string name, DateTime startDate, DateTime endDate, ICollection<Pet> pets)
        : base(id)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        Pets = pets ?? new List<Pet>();
    }
}