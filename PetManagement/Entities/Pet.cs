using System.ComponentModel.DataAnnotations.Schema;

namespace PetManagement.Entities;

public class Pet : BaseEntity<int>
{
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime BirthDate { get; set; }
    public string Color { get; set; }
    public string Gender { get; set; }


    [ForeignKey(nameof(User))]
    public Guid OwnerId { get; set; }
    public virtual User Owner { get; set; }

    public int HealthStatusId { get; set; }
    public virtual HealthStatus HealthStatus { get; set; }

    public virtual ICollection<Activity> Activities { get; set; }
    public virtual ICollection<Food> Foods { get; set; }
    public virtual ICollection<SocialInteraction> SocialInteractions { get; set; }
    public virtual ICollection<Training> Trainings { get; set; }

    public Pet()
    {
        Name = string.Empty;
        Type = string.Empty;
        BirthDate = DateTime.UtcNow;
        Color = string.Empty;
        Gender = string.Empty;
        Activities = new HashSet<Activity>();
        Foods = new HashSet<Food>();
        SocialInteractions = new HashSet<SocialInteraction>();
        Trainings = new HashSet<Training>();
    }
    public Pet(
        string name,
        string type,
        DateTime birthDate,
        string color,
        string gender,
        Guid ownerId
    )
    {
        Name = name;
        Type = type;
        BirthDate = birthDate;
        Color = color;
        Gender = gender;
        OwnerId = ownerId;
    }
    public Pet(
        int id,
        string name,
        string type,
        DateTime birthDate,
        string color,
        string gender,
        Guid ownerId
    )
        : base(id)
    {
        Name = name;
        Type = type;
        BirthDate = birthDate;
        Color = color;
        Gender = gender;
        OwnerId = ownerId;
    }
}
