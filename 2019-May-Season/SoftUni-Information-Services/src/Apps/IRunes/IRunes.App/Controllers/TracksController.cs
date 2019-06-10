using IRunes.App.ViewModels.Tracks;
using IRunes.Models;
using IRunes.Services;
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
        public IActionResult Create(string albumId)
        {
            return this.View(new TrackCreateViewModel{ AlbumId = albumId });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(TrackCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/");
            }

            Track trackForDb = ModelMapper.ProjectTo<Track>(model);

            if (!this.albumService.AddTrackToAlbum(model.AlbumId, trackForDb))
            {
                return this.Redirect("/Albums/All");
            }

            return this.Redirect($"/Albums/Details?id={model.AlbumId}");
        }

        [Authorize]
        public IActionResult Details(TrackDetailsInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect($"Albums/All");
            }

            Track trackFromDb = this.trackService.GetTrackById(model.TrackId);

            if (trackFromDb == null)
            {
                return this.Redirect($"/Albums/Details?id={model.AlbumId}");
            }

            TrackDetailsViewModel trackDetailsViewModel = ModelMapper.ProjectTo<TrackDetailsViewModel>(trackFromDb);

            trackDetailsViewModel.AlbumId = model.AlbumId;

            return this.View(trackDetailsViewModel);
        }
    }
}
