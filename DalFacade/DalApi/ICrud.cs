
namespace DalApi;
using DO;

/// <summary>
/// A generic interface for the CRUD operations.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICrud<T> where T : class
{
    int Create(T entity);//Creating a new object with ID=id.
    T? Read(int id);//Returning an object with ID=id.
    T? Read(Func<T, bool> filter); // stage 2-Returning an object that matches the filter.
    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null); // stage 2-Returning a collection of all objects matching the filter.
    void Update(T entity);//Update the object with ID=T.id.
    void Delete(int id);//Deleting an object with ID=id
}