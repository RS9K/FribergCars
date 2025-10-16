using FribergCars.Models;
using FribergCars.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FribergCars.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly ICarRepository _carRepo;
        public BookingController(IBookingRepository bookingRepo, ICarRepository carRepo)
        {
            _bookingRepo = bookingRepo;
            _carRepo = carRepo;
        }
        // GET: Booking/Create
        [HttpGet]
        public IActionResult Create(int carId)
        {
            // Check if customer is logged in
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login", "Customer");

            var car = _carRepo.GetById(carId);
            if (car == null)
                return NotFound();

            var booking = new Booking
            {
                CarId = car.Id,
                Car = car,
                StartDate = DateTime.Now, // Default to current date
                EndDate = DateTime.Now.AddDays(1) // Default to one day later
            };

            return View(booking); // Pass booking to the view
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Booking booking)
        {
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login", "Customer");

            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
                return RedirectToAction("Login", "Customer");

            if (booking.StartDate >= booking.EndDate)
            {
                ModelState.AddModelError("", "End date must be after start date.");
                booking.Car = _carRepo.GetById(booking.CarId);
                return View(booking);
            }

            booking.CustomerId = customerId.Value;
            _bookingRepo.Add(booking);

            TempData["BookingSuccess"] = "The Booking was successfull!";
            return RedirectToAction("MyBookings", "Customer");
        }

    }
}
