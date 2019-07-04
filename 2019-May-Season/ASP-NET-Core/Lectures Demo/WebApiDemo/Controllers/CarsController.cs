using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDemo.Data;

namespace WebApiDemo.Controllers
{
    public class CarsController : ApiController
    {
        private readonly ApplicationDbContext dbContext;

        public CarsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Car>> Get()
        {
            return this.dbContext.Cars.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Car> Get(int id)
        {
            var car = this.dbContext.Cars.FirstOrDefault(x => x.Id == id);
            return car;
        }

        [HttpPost]
        public async Task<ActionResult<Car>> Post(Car car)
        {
            await this.dbContext.Cars.AddAsync(car);
            await this.dbContext.SaveChangesAsync();
            return this.CreatedAtAction("Get", new { id = car.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Car car, int id)
        {
            var dbCar = this.dbContext.Cars.FirstOrDefault(x => x.Id == id);
            dbCar.Color = car.Color;
            dbCar.Model = car.Model;
            dbCar.Year = car.Year;
            await dbContext.SaveChangesAsync();
            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Car>> Delete(int id)
        {
            var car = this.dbContext.Cars.FirstOrDefault(x => x.Id == id);
            if (car == null)
            {
                return this.NotFound();
            }

            this.dbContext.Cars.Remove(car);
            await this.dbContext.SaveChangesAsync();
            return car;
        }
    }
}
