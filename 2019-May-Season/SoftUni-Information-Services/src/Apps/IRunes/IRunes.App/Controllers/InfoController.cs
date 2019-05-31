using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Action;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class InfoController : Controller
    {
        public int MyProperty { get; set; }

        [NonAction]
        public override string ToString()
        {
            return base.ToString();
        }

        public ActionResult Json(IHttpRequest request)
        {
            return Json(new { });
        }

        public ActionResult File(IHttpRequest request)
        {
            string folderPrefix = "/../";
            string assemblyLocation = this.GetType().Assembly.Location;
            string resourceFolderPath = "Resources/";
            string requestedResource = request.QueryData["file"].ToString();

            string fullPathToResource = assemblyLocation + folderPrefix + resourceFolderPath + requestedResource;

            if (System.IO.File.Exists(fullPathToResource))
            {
                // TODO: Students, Do this!!!
                string mimeType = null;
                string fileName = null;

                byte[] content = System.IO.File.ReadAllBytes(fullPathToResource);
                return File(content);
            }

            return NotFound();
        }

        public IHttpResponse About(IHttpRequest request)
        {
            return this.View();
        }
    }
}
