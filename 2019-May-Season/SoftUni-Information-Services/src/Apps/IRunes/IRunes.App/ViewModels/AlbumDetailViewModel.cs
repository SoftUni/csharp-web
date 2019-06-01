using System.Collections.Generic;

namespace IRunes.App.ViewModels
{
    public class AlbumDetailsViewModel
    {
        public AlbumDetailsViewModel()
        {
            this.Tracks = new List<TrackAlbumAllViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public string Price { get; set; }

        public List<TrackAlbumAllViewModel> Tracks { get; set; }
    }
}
