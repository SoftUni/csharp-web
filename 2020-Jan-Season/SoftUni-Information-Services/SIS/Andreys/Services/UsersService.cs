namespace Andreys.Services
{
    using Andreys.Data;
    using Andreys.Models;

    using SIS.MvcFramework;
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class UsersService : IUsersService
    {
        private readonly AndreysDbContext db;

        public UsersService(AndreysDbContext db)
        {
            this.db = db;
        }

        public bool EmailExists(string email)
        {
            return this.db.Users.Any(x => x.Email == email);
        }

        public string GetUserId(string username, string password)
        {
            var hashPassword = this.Hash(password);
            var user = this.db.Users.FirstOrDefault(
                u => u.Username == username && u.Password == hashPassword);
            if (user == null)
            {
                return null;
            }

            return user.Id;
        }

        public string GetUsername(string id)
        {
            var username = this.db.Users
                .Where(x => x.Id == id)
                .Select(x => x.Username)
                .FirstOrDefault();
            return username;
        }

        public void Register(string username, string email, string password)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Role = IdentityRole.User,
                Username = username,
                Email = email,
                Password = this.Hash(password),
            };

            this.db.Users.Add(user);
            this.db.SaveChanges();
        }

        public bool UsernameExists(string username)
        {
            return this.db.Users.Any(x => x.Username == username);
        }

        private string Hash(string input)
        {
            if (input == null)
            {
                return null;
            }

            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
