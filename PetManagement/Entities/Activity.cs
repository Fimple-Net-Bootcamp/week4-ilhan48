namespace PetManagement.Entities;

public class Activity : BaseEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string DifficultyLevel { get; set; }

    public virtual ICollection<Pet> Pets { get; set; }

    public Activity()
    {
        Name = string.Empty;
        Description = string.Empty;
        DifficultyLevel = string.Empty;
    }

    public Activity(
        string name,
        string description,
        string difficultyLevel
    )
    {
        Name = name;
        Description = description;
        DifficultyLevel = difficultyLevel;
    }
    public Activity(
        int id,
        string name,
        string description,
        string difficultyLevel
    )
        : base(id)
    {
        Name = name;
        Description = description;
        DifficultyLevel = difficultyLevel;
    }
}