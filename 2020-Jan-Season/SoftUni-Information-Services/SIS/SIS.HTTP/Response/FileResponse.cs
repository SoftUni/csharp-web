namespace SIS.HTTP.Response
{
    /// <summary>
    /// Represents a File Response with properties for the <c>Response Status Line</c>, <c>Response Headers</c> and <c>Response Body</c>.
    /// </summary>
    public class FileResponse : HttpResponse
    {
        /// <summary>
        /// Initializes a new <see cref="FileResponse"/> class.
        /// </summary>
        /// <param name="fileContent">File bytes</param>
        /// <param name="contentType">MIME Content type</param>
        public FileResponse(byte[] fileContent, string contentType)
        {
            this.StatusCode = HttpResponseCode.Ok;
            this.Body = fileContent;
            this.Headers.Add(new Header("Content-Type", contentType));
            this.Headers.Add(new Header("Content-Length", this.Body.Length.ToString()));
        }
    }
}
