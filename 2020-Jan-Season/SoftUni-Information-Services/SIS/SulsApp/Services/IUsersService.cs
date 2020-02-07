namespace SulsApp.Services
{
    public interface IUsersService
    {
        void CreateUser(string username, string email, string password);

        string GetUserId(string username, string password);

        void ChangePassword(string username, string newPassword);

        int CountUsers();
    }
}
