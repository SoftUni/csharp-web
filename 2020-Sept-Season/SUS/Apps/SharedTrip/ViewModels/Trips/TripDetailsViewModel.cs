namespace SharedTrip.ViewModels.Trips
{
    public class TripDetailsViewModel : TripViewModel
    {
        public string ImagePath { get; set; }

        public string Description { get; set; }

        public string DepartureTimeFormatted => this.DepartureTime.ToString("s");
    }
}
