namespace PetManagement.Shared;

public class CommandResult<TId>
{
    public TId Id { get; set; }
    public List<string> Errors { get; set; }
}
