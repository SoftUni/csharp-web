using SulsApp.Models;
using SulsApp.ViewModels.Home;
using SulsApp.ViewModels.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SulsApp.Services
{
    public class ProblemsService : IProblemsService
    {
        private readonly ApplicationDbContext db;

        public ProblemsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void CreateProblem(string name, int points)
        {
            var problem = new Problem
            {
                Name = name,
                Points = points,
            };
            this.db.Problems.Add(problem);
            this.db.SaveChanges();
        }

        public DetailsViewModel GetDetailsProblems(string id)
        {
            var viewModel = this.db.Problems
               .Where(x => x.Id == id)
               .Select(x => new DetailsViewModel
               {
                   Name = x.Name,
                   Problems = x.Submissions.Select(s => new ProblemDetailsSubmissionViewModel
                   {
                       Username = s.User.Username,
                       AchievedResult = s.AchievedResult,
                       MaxPoints = x.Points,
                       CreatedOn = s.CreatedOn,
                       SubmissionId = s.Id
                   })
                   .ToList()
               })
               .FirstOrDefault();

            return viewModel;
        }

        public IEnumerable<IndexProblemViewModel> GetProblems()
        {
            var problems = this.db.Problems.Select(x => new IndexProblemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Count = x.Submissions.Count
            }).ToList();

            return problems;
        }
    }
}
