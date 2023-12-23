using System.ComponentModel.DataAnnotations.Schema;
using PetManagement.Database.Repositories.Core;

namespace PetManagement.Entities;

public class Food : BaseEntity<int>
{
    public string Name { get; set; }
    public virtual ICollection<Pet> Pets { get; set; }

    public Food()
    {
        Name = string.Empty;
    }
    public Food(string name)
    {
        Name = name;
    }
    public Food(int id, string name) : base(id)
    {
        Name = name;
    }
}
