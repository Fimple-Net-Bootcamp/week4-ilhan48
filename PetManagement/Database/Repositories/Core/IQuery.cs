namespace PetManagement.Database.Repositories.Core;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
