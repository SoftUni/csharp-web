using System.Collections.Generic;
using System.IO;
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

        public ActionResult Json()
        {
            return Json(new { });
        }

        public ActionResult File()
        {
            string folderPrefix = "/../";
            string assemblyLocation = this.GetType().Assembly.Location;
            string resourceFolderPath = "Resources/";
            string requestedResource = this.Request.QueryData["file"].ToString();

            string fullPathToResource = assemblyLocation + folderPrefix + resourceFolderPath + requestedResource;

            if (System.IO.File.Exists(fullPathToResource))
            {
                // TODO: Students, Do this!!!
                FileInfo resourceFile = new FileInfo(fullPathToResource);
                string fileName = resourceFile.Name;

                byte[] content = System.IO.File.ReadAllBytes(fullPathToResource);
                return File(content, fileName);
            }

            return NotFound();
        }

        public IHttpResponse About()
        {
            return this.View();
        }
    }
}
