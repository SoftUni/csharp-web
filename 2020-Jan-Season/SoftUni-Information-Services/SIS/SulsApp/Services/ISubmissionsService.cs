using System;
using System.Collections.Generic;
using System.Text;

namespace SulsApp.Services
{
    public interface ISubmissionsService
    {
        void Create(string userId, string problemId, string code);

        void Delete(string id);
    }
}
