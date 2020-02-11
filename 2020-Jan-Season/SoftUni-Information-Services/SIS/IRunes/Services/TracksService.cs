using IRunes.Models;
using IRunes.ViewModels.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunes.Services
{
    public class TracksService : ITracksService
    {
        private readonly ApplicationDbContext db;

        public TracksService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string albumId, string name, string link, decimal price)
        {
            var track = new Track
            {
                AlbumId = albumId,
                Name = name,
                Link = link,
                Price = price,
            };

            this.db.Tracks.Add(track);

            var allTrackPricesSum = this.db.Tracks
                .Where(x => x.AlbumId == albumId)
                .Sum(x => x.Price) + price;
            var album = this.db.Albums.Find(albumId);
            album.Price = allTrackPricesSum * 0.87M;

            this.db.SaveChanges();
        }

        public DetailsViewModel GetDetails(string trackId)
        {
            var track = this.db.Tracks.Where(x => x.Id == trackId)
                .Select(x => new DetailsViewModel
                {
                    Name = x.Name,
                    Link = x.Link,
                    AlbumId = x.AlbumId,
                    Price = x.Price,
                }).FirstOrDefault();
            
            return track;
        }
    }
}
