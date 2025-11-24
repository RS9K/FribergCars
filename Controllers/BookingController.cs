using FribergCars.Models;
using FribergCars.Services;
using Microsoft.AspNetCore.Mvc;

namespace FribergCars.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingApiClient _bookingApi;
        private readonly CarApiClient _carApi;
        public BookingController(BookingApiClient bookingApi, CarApiClient carApi)
        {
            _bookingApi = bookingApi;
            _carApi = carApi;
        }
        // GET: Booking/Create
        [HttpGet]
        public async Task<IActionResult> Create(int carId)
        {
            // Check if customer is logged in
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login", "Customer");

            var car = await _carApi.GetByIdAsync(carId);
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
        public async Task<IActionResult> Create(Booking booking)
        {
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login", "Customer");

            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
                return RedirectToAction("Login", "Customer");

            if (booking.StartDate >= booking.EndDate)
            {
                ModelState.AddModelError("", "End date must be after start date.");
                booking.Car = await _carApi.GetByIdAsync(booking.CarId);
                return View(booking);
            }

            booking.CustomerId = customerId.Value;
            var created = await _bookingApi.CreateAsync(booking);
            if (created == null)
            {
                ModelState.AddModelError("", "Failed to create booking. Please try again.");
                booking.Car = await _carApi.GetByIdAsync(booking.CarId);
                return View(booking);
            }

            TempData["BookingSuccess"] = "The Booking was successfull!";
            return RedirectToAction("MyBookings", "Customer");
        }

    }
}
