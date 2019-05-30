using System.Linq;
using IRunes.Models;
using SIS.MvcFramework.Extensions;

namespace IRunes.App.Extensions
{
    public static class EntityExtensions
    {
        private static string GetTracks(Album album)
        {
            return
                album.Tracks.Count == 0
                    ? "There are currently not tracks in this album."
                    : string.Join("", album.Tracks.Select((track, index) => track.ToHtmlAll(album.Id, index + 1)));
        }

        public static string ToHtmlAll(this Album album)
        {
            return $"<h3><a href=\"/Albums/Details?id={album.Id}\">{album.Name.Decode()}</a></h3>";
        }

        public static string ToHtmlDetails(this Album album)
        {
            return "<div class=\"album-details d-flex justify-content-between row\">" +
                   "    <div class=\"album-data col-md-5\">" +
                   $"       <img src=\"{album.Cover.Decode()}\" class=\"img-thumbnail\"/>" +
                   $"       <h1 class=\"text-center\">Album Name: {album.Name.Decode()}</h1>" +
                   $"       <h1 class=\"text-center\">Album Price: ${album.Price:F2}</h1>" +
                   "        <div class=\"d-flex justify-content-between\">" +
                   $"           <a class=\"btn bg-success text-white\" href=\"/Tracks/Create?albumId={album.Id}\">Create Track</a>" +
                   "            <a class=\"btn bg-success text-white\" href=\"/Albums/All\">Back To All</a>" +
                   "        </div>" +
                   "    </div>" +
                   "    <div class=\"album-tracks col-md-6\">" +
                   "        <h1>Tracks</h1>" +
                   $"       {GetTracks(album)}" +
                   "    </div>" +
                   "</div>";
        }

        public static string ToHtmlAll(this Track track, string albumId, int index)
        {
            return $"<li><strong>{index}</strong>. <a href=\"/Tracks/Details?albumId={albumId}&trackId={track.Id}\"><i>{track.Name.Decode()}</i></a></li>";
        }

        public static string ToHtmlDetails(this Track track, string albumId)
        {
            return "<div class=\"track-details\">" +
                   $"    <h4 class=\"text-center\">Track Name: {track.Name.Decode()}</h4>" +
                   $"    <h4 class=\"text-center\">Track Price: ${track.Price:F2}</h4>" +
                   "    <hr class=\"bg-success w-50\" style=\"height: 2px\" />" +
                   "    <div class=\"d-flex justify-content-center\">" +
                   $"        <iframe src=\"{track.Link.Decode()}\" class=\"w-50\" height=\"480\"></iframe>" +
                   "    </div>" +
                   "    <hr class=\"bg-success w-50\" style=\"height: 2px\" />" +
                   "    <div class=\"d-flex justify-content-center\">" +
                   $"        <a href=\"/Albums/Details?id={albumId}\" class=\"btn bg-success text-white\">Back To Album</a>" +
                   "    </div>" +
                   "</div>";
        }
    }
}
