using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarsAPI.Models;

namespace CarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        CarsContext CarDb = new CarsContext();

        //this will create a list of Cars from the imported Database and Give that list to all other Methods
        [HttpGet]
        [Route("CarList")]
        public List<Car> PopulateList()
        {
            List<Car> carsList = CarDb.Cars.ToList();
            return carsList;
        }


        //this will search the Database list of cars and match all cars with the given Make
        [HttpGet]
        [Route("DisplayMake/{name}")]
        public List<Car> SearchByMake(string name)
        {
            List<Car> carList = PopulateList();
            List<Car> output = carList.Where(x => x.Make.ToLower() == name.ToLower()).ToList();
            return output;
        }

        //this will search the Database and return a list of cars with the matching Model
        [HttpGet]
        [Route("DisplayModel/{name}")]
        public List<Car> SearchByModel(string name)
        {
            List<Car> carList = PopulateList();

            List<Car> output = carList.Where(x => x.Model.ToLower() == name.ToLower()).ToList();
            return output;
        }

        //this will search the Database and return a list of cars that match the year
        [HttpGet]
        [Route("DisplayListYear/{year}")]
        public List<Car> SearchByYear(int year)
        {
            List<Car> carList = PopulateList();
            List<Car> output = carList.Where(x => x.Year == year).ToList();

            return output;
        }

        //this will search the Database and return a list of cars matching the Color
        [HttpGet]
        [Route("DisplayListColor/{color}")]
        public List<Car> SearchByColor(string color)
        {
            List<Car> carList = PopulateList();
            List<Car> output = carList.Where(x => x.Color.ToLower() == color.ToLower()).ToList();
            return output;
        }


        private readonly CarsContext _context;

        public CarsController(CarsContext context)
        {
            _context = context;
        }

        // GET: api/Cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars.ToListAsync();
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cars
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { id = car.Id }, car);
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Car>> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return car;
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
