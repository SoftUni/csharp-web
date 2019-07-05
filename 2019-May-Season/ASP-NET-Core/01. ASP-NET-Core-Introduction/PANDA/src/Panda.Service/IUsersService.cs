using Panda.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Panda.Services
{
    public interface IUsersService
    {
        List<PandaUser> GetAllUsers();

        PandaUser GetUser(string username);
    }
}
