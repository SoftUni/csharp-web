using IRunes.Services;
using IRunes.ViewModels.Tracks;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITracksService tracksService;

        public TracksController(ITracksService tracksService)
        {
            this.tracksService = tracksService;
        }

        public HttpResponse Create(string albumId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = new CreateViewModel { AlbumId = albumId };
            return this.View(viewModel);
        }

        [HttpPost]
        public HttpResponse Create(CreateInputModel input)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (input.Name.Length < 4 || input.Name.Length > 20)
            {
                return this.Error("Track name should be between 4 and 20 characters.");
            }

            if (!input.Link.StartsWith("http"))
            {
                return this.Error("Invalid link.");
            }

            if (input.Price < 0)
            {
                return this.Error("Price should be a positive number.");
            }

            this.tracksService.Create(input.AlbumId, input.Name, input.Link, input.Price);
            return this.Redirect("/Albums/Details?id=" + input.AlbumId);
        }

        public HttpResponse Details(string trackId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.tracksService.GetDetails(trackId);
            if (viewModel == null)
            {
                return this.Error("Track not found.");
            }

            return this.View(viewModel);
        }
    }
}
