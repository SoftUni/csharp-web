namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        /// <summary>
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <param name="isUpVote">If true - up vote, else - down vote.</param>
        /// <returns></returns>
        Task VoteAsync(int postId, string userId, bool isUpVote);

        int GetVotes(int postId);
    }
}
