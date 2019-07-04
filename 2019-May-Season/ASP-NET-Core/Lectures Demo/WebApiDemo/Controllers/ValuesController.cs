using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Configuration;

namespace WebApiDemo.Controllers
{
    public class ValuesController : ApiController
    {

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "";
        }

        // GET api/values/GetCar/2019
        [HttpGet("GetCar/{year:int}")]
        public ActionResult<Car> GetCar(int year)
        {
            if (year > DateTime.UtcNow.Year)
            {
                return this.BadRequest();
            }

            return new Car { Color = Color.Black, Model = "Audi A7", Year = year };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return this.BadRequest();
        }
    }
}
