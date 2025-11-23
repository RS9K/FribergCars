using Microsoft.AspNetCore.Mvc;
using FribergCars.Models;
using FribergCars.Services;

namespace FribergCars.Controllers
{
    public class CarController : Controller
    {
        private readonly CarApiClient _carApiClient;

        public CarController (CarApiClient carApiClient)
        {
            _carApiClient = carApiClient;
        }

        // GET: /Car
        public async Task<IActionResult> Index()
        {
            var cars = await _carApiClient.GetAllAsync();
            return View(cars); 
            // Views/Car/Index.cshtml
        }

        // GET: /Car/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var car = await _carApiClient.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
            // Views/Car/Details.cshtml
        }
    }
}
