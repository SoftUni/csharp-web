using Microsoft.AspNetCore.Identity;
using MyFirstMvcApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMvcApp.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;

        public UsersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int GetCount()
        {
            return this.dbContext.Users.Count();
        }

        public IEnumerable<string> GetUsernames()
        {
            return this.dbContext.Users.Select(x => x.UserName).ToList();
        }

        public string LatestUsername(string orderBy = "username")
        {
            IQueryable<IdentityUser> query = this.dbContext.Users;
            if (orderBy == "username")
            {
                query = query.OrderByDescending(x => x.UserName);
            }
            else if (orderBy == "email")
            {
                query = query.OrderByDescending(x => x.Email);
            }

            return query.Select(x => x.UserName).FirstOrDefault();
        }
    }
}
