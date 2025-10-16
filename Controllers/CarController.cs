using Microsoft.AspNetCore.Mvc;
using FribergCars.Models;
using FribergCars.Data;
using FribergCars.Repositories.Interfaces;

namespace FribergCars.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarRepository _carRepo;

        public CarController (ICarRepository carRepository)
        {
            _carRepo = carRepository;
        }

        // GET: /Car
        public IActionResult Index()
        {
            var cars = _carRepo.GetAll().Where(c => c.IsAvailable).ToList();
            return View(cars); 
            // Views/Car/Index.cshtml
        }

        // GET: /Car/Details/5
        public IActionResult Details(int id)
        {
            var car = _carRepo.GetById(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
            // Views/Car/Details.cshtml
        }
    }
}
