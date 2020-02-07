using System;
using System.Collections.Generic;
using System.Text;

namespace SulsApp.Services
{
    public interface IProblemsService
    {
        void CreateProblem(string name, int points);
    }
}
