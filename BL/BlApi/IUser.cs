using BO;

namespace BlApi
{
    public interface IUser
    {
        int Create(User user);
        User Read(int engineerId);
        IEnumerable<User> ReadAll(Func<User, bool>? filter = null);
        void Update(User user);
        void Delete(int engineerId);
    }
}
