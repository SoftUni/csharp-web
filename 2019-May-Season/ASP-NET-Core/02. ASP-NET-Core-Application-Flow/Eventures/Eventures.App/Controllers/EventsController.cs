using Eventures.App.Models.BindingModels;
using Eventures.App.Models.ViewModels;
using Eventures.Data;
using Eventures.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Eventures.App.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventuresDbContext context;

        public EventsController(EventuresDbContext context)
        {
            this.context = context;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(EventCreateBindingModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                Event eventForDb = new Event
                {
                    Name = bindingModel.Name,
                    Place = bindingModel.Place,
                    Start = bindingModel.Start,
                    End = bindingModel.End,
                    TotalTickets = bindingModel.TotalTickets,
                    PricePerTicket = bindingModel.PricePerTicket
                };

                context.Events.Add(eventForDb);
                context.SaveChanges();

                return this.RedirectToAction("All");
            }

            return this.View();
        }

        public IActionResult All()
        {
            List<EventAllViewModel> events = context.Events
                .Select(eventFromDb => new EventAllViewModel
                {
                    Name = eventFromDb.Name,
                    Place = eventFromDb.Place,
                    Start = eventFromDb.Start.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture),
                    End = eventFromDb.End.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture)
                })
                .ToList();

            return this.View(events);
        }
    }
}