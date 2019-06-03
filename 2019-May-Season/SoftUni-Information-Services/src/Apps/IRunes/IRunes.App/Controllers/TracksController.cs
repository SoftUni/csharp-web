using System.Collections.Generic;
using System.Linq;
using IRunes.App.ViewModels;
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
        public ActionResult Create()
        {
            string albumId = this.Request.QueryData["albumId"].FirstOrDefault();

            return this.View(new TrackCreateViewModel{ AlbumId = albumId });
        }

        [Authorize]
        [HttpPost(ActionName = "Create")]
        public ActionResult CreateConfirm()
        {
            string albumId = this.Request.QueryData["albumId"].FirstOrDefault();
            string name = this.Request.FormData["name"].FirstOrDefault();
            string link = this.Request.FormData["link"].FirstOrDefault();
            string price = this.Request.FormData["price"].FirstOrDefault();

            Track trackForDb = new Track
            {
                Name = name,
                Link = link,
                Price = decimal.Parse(price)
            };

            if (!this.albumService.AddTrackToAlbum(albumId, trackForDb))
            {
                return this.Redirect("/Albums/All");
            }

            return this.Redirect($"/Albums/Details?id={albumId}");
        }

        [Authorize]
        public ActionResult Details()
        {
            string albumId = this.Request.QueryData["albumId"].FirstOrDefault();
            string trackId = this.Request.QueryData["trackId"].FirstOrDefault();

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
