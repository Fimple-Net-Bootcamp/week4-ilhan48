namespace PetManagement.Entities;

public class Training : BaseEntity<int>
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public virtual ICollection<Pet> Pets { get; set; }

    public Training()
    {
        Name = string.Empty;
        Date = DateTime.UtcNow;
    }

    public Training(string name, DateTime date)
    {
        Name = name;
        Date = date;
    }

    public Training(int id, string name, DateTime date, int petId)
        : base(id)
    {
        Name = name;
        Date = date;
    }
}