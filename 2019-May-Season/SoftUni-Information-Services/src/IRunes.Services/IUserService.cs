using IRunes.Models;

namespace IRunes.Services
{
    public interface IUserService
    {
        User CreateUser(User user);

        User GetUserByUsernameAndPassword(string username, string password);
    }
}
