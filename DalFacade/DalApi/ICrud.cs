namespace DalApi;
using DO;

public interface ICrud<T> where T : class
{
    int Create(T entity);
    T? Read(int id);
    T? Read(Func<T, bool> filter); // stage 2
    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null); // stage 2
    void Update(T entity);
    void Delete(int id);
}