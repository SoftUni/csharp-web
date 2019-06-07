namespace IRunes.App.ViewModels.Tracks
{
    using SIS.MvcFramework.Attributes.Validation;

    public class TrackDetailsInputModel
    {
        [RequiredSis]
        public string AlbumId { get; set; }

        [RequiredSis]
        public string TrackId { get; set; }
    }
}
