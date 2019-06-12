using System.Linq;
using Musaca.Data;
using Musaca.Models;

namespace Musaca.Services
{
    public class UserService : IUserService
    {
        private readonly MusacaDbContext context;

        public UserService(MusacaDbContext musacaDbContext)
        {
            this.context = musacaDbContext;
        }

        public User CreateUser(User user)
        {
            user = this.context.Users.Add(user).Entity;
            this.context.SaveChanges();

            return user;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return this.context.Users.SingleOrDefault(user => (user.Username == username || user.Email == username)
                                                              && user.Password == password);
        }
    }
}
