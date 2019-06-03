using System.Collections.Generic;
using System.Linq;
using IRunes.App.ViewModels;
using IRunes.App.ViewModels.Tracks;
using IRunes.Models;
using IRunes.Services;
using Microsoft.Extensions.Logging;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Mapping;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITrackService trackService;

        private readonly IAlbumService albumService;

        public TracksController(ITrackService trackService, IAlbumService albumService)
        {
            this.trackService = trackService;
            this.albumService = albumService;
        }

        [Authorize]
        public ActionResult Create(string albumId)
        {
            return this.View(new TrackCreateViewModel{ AlbumId = albumId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(CreateInputModel model)
        {
            Track trackForDb = new Track
            {
                Name = model.Name,
                Link = model.Link,
                Price = model.Price,
            };

            if (!this.albumService.AddTrackToAlbum(model.AlbumId, trackForDb))
            {
                return this.Redirect("/Albums/All");
            }

            return this.Redirect($"/Albums/Details?id={model.AlbumId}");
        }

        [Authorize]
        public ActionResult Details(string albumId, string trackId)
        {
            Track trackFromDb = this.trackService.GetTrackById(trackId);

            if (trackFromDb == null)
            {
                return this.Redirect($"/Albums/Details?id={albumId}");
            }

            TrackDetailsViewModel trackDetailsViewModel = ModelMapper.ProjectTo<TrackDetailsViewModel>(trackFromDb);
            trackDetailsViewModel.AlbumId = albumId;

            return this.View(trackDetailsViewModel);
        }
    }
}
