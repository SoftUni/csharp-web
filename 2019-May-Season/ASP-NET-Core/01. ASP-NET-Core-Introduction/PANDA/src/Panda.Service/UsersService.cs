using Panda.Data;
using Panda.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panda.Services
{
    public class UsersService : IUsersService
    {
        private readonly PandaDbContext pandaDbContext;

        public UsersService(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public List<PandaUser> GetAllUsers()
        {
            List<PandaUser> users = this.pandaDbContext.Users.ToList();

            return users;
        }

        public PandaUser GetUser(string username)
        {
            PandaUser userDb = this.pandaDbContext.Users.SingleOrDefault(user => user.UserName == username);

            return userDb;
        }
    }
}
