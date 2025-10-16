using Microsoft.AspNetCore.Mvc;
using FribergCars.Models;
using FribergCars.Data;
using FribergCars.Repositories.Interfaces;
using Microsoft.Identity.Client;

namespace FribergCars.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly ICarRepository _carRepo;

        public CustomerController (ICustomerRepository customerRepo, IBookingRepository bookingRepo, ICarRepository carRepo)
        {
            _customerRepo = customerRepo;
            _bookingRepo = bookingRepo;
            _carRepo = carRepo;
        }

        // GET: Customer/Register
        public IActionResult Register()
        {
             return View(); // Views/Customer/Register.cshtml
        }

        // POST: Customer/Register
        [HttpPost]
        public IActionResult Register (string fullName, string email, string password)
        {
            var existing = _customerRepo.GetByEmail(email);
            if (existing != null)
            {
                ViewData["Error"] = "Email already registered.";
                return View(); // Views/Customer/Register.cshtml
            }

            var customer = new Customer
            {
                FullName = fullName,
                Email = email,
                Password = password 
            };
            _customerRepo.Add(customer);

            // Auto-login after registration
            HttpContext.Session.SetString("UserType", "Customer");
            HttpContext.Session.SetInt32("UserId", customer.Id);
            return RedirectToAction("CustomerHome", "Home"); // Redirect to home page after registration
        }

        // GET: Customer/Login
        public IActionResult Login()
        {
            return View(); // Views/Customer/Login.cshtml
        }

        // POST: Customer/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var customer = _customerRepo.GetByEmailAndPassword(email, password);
            if (customer == null)
            {
                ViewData["Error"] = "Invalid email or password.";
                return View();
            }

            HttpContext.Session.SetString("UserType", "Customer");
            HttpContext.Session.SetInt32("CustomerId", customer.Id);
            return RedirectToAction("CustomerHome", "Home"); // Redirect to home page after login
        }

        // GET: Customer/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session data
            return RedirectToAction("Index", "Home"); // Redirect to home page after logout
        }
        // GET: Customer/MyBookings
        [HttpGet("book-car/{id}")]
        public IActionResult BookCar(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login");

            var car = _carRepo.GetById(id);
            if (car == null)
                return NotFound();

            var booking = new Booking
            {
                CarId = car.Id,
                Car = car
            };

            return View(booking);
        }
        [HttpPost("book-car/{id}")]
        public IActionResult BookCar(int id, Booking booking)
        {
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login");

            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
                return RedirectToAction("Login");

            booking.CustomerId = customerId.Value;
            booking.CarId = id;
            _bookingRepo.Add(booking);

            return RedirectToAction("MyBookings");
        }
        [HttpGet("my-bookings")]
        public IActionResult MyBookings()
        {
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login");

            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
                return RedirectToAction("Login");

            var bookings = _bookingRepo.GetByCustomerId(customerId.Value);
            return View("MyBookings", bookings); // Views/Customer/MyBookings.cshtml
        }

        // Delete Booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBooking(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login");

            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
                return RedirectToAction("Login");

            var booking = _bookingRepo.GetById(id);

            if (booking == null || booking.CustomerId != customerId.Value)
                return Unauthorized(); // Prevents deleting others’ bookings

            _bookingRepo.Delete(id);

            return RedirectToAction("MyBookings");
        }
    }
}
