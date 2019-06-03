using System.Linq;
using IRunes.Data;
using IRunes.Models;

namespace IRunes.Services
{
    public class TrackService : ITrackService
    {
        private readonly RunesDbContext context;

        public TrackService(RunesDbContext runesDbContext)
        {
            this.context = runesDbContext;
        }

        public Track GetTrackById(string trackId)
        {
            return this.context.Tracks
                .SingleOrDefault(track => track.Id == trackId);
        }
    }
}
