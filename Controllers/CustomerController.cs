using Microsoft.AspNetCore.Mvc;
using FribergCars.Models;
using FribergCars.Services;

namespace FribergCars.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerApiClient _customerApi;
        private readonly CarApiClient _carApi;
        private readonly BookingApiClient _bookingApi;

        public CustomerController(
            CustomerApiClient customerApi,
            CarApiClient carApi,
            BookingApiClient bookingApi)
        {
            _customerApi = customerApi;
            _carApi = carApi;
            _bookingApi = bookingApi;
        }

        // GET: Customer/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Customer/Register
        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password)
        {
            var customer = new Customer
            {
                FullName = fullName,
                Email = email,
                Password = password
            };

            var created = await _customerApi.RegisterAsync(customer);

            if (created == null)
            {
                ViewData["Error"] = "Email already registered";
                return View();
            }

            HttpContext.Session.SetString("UserType", "Customer");
            HttpContext.Session.SetInt32("CustomerId", created.Id);

            return RedirectToAction("CustomerHome", "Home");
        }

        // GET: Customer/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Customer/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var customer = await _customerApi.LoginAsync(email, password);

            if (customer == null)
            {
                ViewData["Error"] = "Invalid login";
                return View();
            }

            HttpContext.Session.SetString("UserType", "Customer");
            HttpContext.Session.SetInt32("CustomerId", customer.Id);

            return RedirectToAction("CustomerHome", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Book-Car
        public async Task<IActionResult> BookCar(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login");

            var car = await _carApi.GetByIdAsync(id);
            if (car == null)
                return NotFound();

            var booking = new Booking
            {
                CarId = car.Id,
                Car = car
            };

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> BookCar(int id, Booking booking)
        {
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login");

            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
                return RedirectToAction("Login");

            booking.CustomerId = customerId.Value;
            booking.CarId = id;

            await _bookingApi.CreateAsync(booking);

            return RedirectToAction("MyBookings");
        }

        public async Task<IActionResult> MyBookings()
        {
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login");

            int customerId = HttpContext.Session.GetInt32("CustomerId")!.Value;

            var bookings = await _bookingApi.GetByCustomerIdAsync(customerId);

            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Customer")
                return RedirectToAction("Login");

            await _bookingApi.DeleteAsync(id);

            return RedirectToAction("MyBookings");
        }
    }
}