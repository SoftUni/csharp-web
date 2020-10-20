namespace SharedTrip.Services
{
    public interface IUsersService
    {
        string GetUserId(string username, string password);

        void Create(string username, string email, string password);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}
