using IRunes.Models;

namespace IRunes.Services
{
    public interface ITrackService
    {
        Track GetTrackById(string trackId);
    }
}
