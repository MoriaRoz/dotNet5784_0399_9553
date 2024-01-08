namespace DalApi;
using DO;

public interface ICrud<T> where T : class
{
    int Create(T entity);
    T? Read(int id);
    List<T> ReadAll();
    void Update(T entity);
    void Delete(int id);
}