namespace SIS.HTTP
{
    using System.Threading.Tasks;

    /// <summary>
    /// Describes HTTP Server functionalities.
    /// </summary>
    public interface IHttpServer
    {
        /// <summary>
        /// Starts the HTTP Server asynchronously.
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Resets the HTTP Server asynchronously.
        /// </summary>
        Task ResetAsync();

        /// <summary>
        /// Stops the HTTP Server.
        /// </summary>
        void Stop();
    }
}
