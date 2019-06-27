using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMvcApp.Services
{
    public interface IUsersService
    {
        int GetCount();

        IEnumerable<string> GetUsernames();

        string LatestUsername(string orderBy = "username");
    }
}
