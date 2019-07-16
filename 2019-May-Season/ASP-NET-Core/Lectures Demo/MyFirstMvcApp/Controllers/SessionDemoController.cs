using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstMvcApp.Controllers
{
    [Serializable]
    public class Name
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class SessionDemoController : Controller
    {
        public IActionResult Save()
        {
            var name = new Name { FirstName = "Niki", LastName = "Kostov" };
            var nameAsString = JsonConvert.SerializeObject(name);
            var nameAsByteArray = Encoding.UTF8.GetBytes(nameAsString);

            this.HttpContext.Session.Set("key", nameAsByteArray);
            return this.Ok();
        }

        public IActionResult Load()
        {
            this.HttpContext.Session.TryGetValue("key", out var data);
            var nameAsString = Encoding.UTF8.GetString(data);
            var name = JsonConvert.DeserializeObject<Name>(nameAsString);
            return this.Content(name.FirstName + " " + name.LastName);
        }
    }
}
