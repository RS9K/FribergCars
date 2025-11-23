using FribergCars.Models;
using FribergCars.Services;
using Microsoft.AspNetCore.Mvc;

namespace FribergCars.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly AdminApiClient _adminApi;
        private readonly CarApiClient _carApi;
        private readonly CustomerApiClient _customerApi;
        private readonly BookingApiClient _bookingApi;

        public AdminController(
            AdminApiClient adminApi,
            CarApiClient carApi,
            CustomerApiClient customerApi,
            BookingApiClient bookingApi)
        {
            _adminApi = adminApi;
            _carApi = carApi;
            _customerApi = customerApi;
            _bookingApi = bookingApi;
        }

        // GET: /admin/
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // GET: /admin/login
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /admin/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var admin = await _adminApi.LoginAsync(email, password);

            if (admin == null)
            {
                ViewData["Error"] = "Invalid email or password.";
                return View();
            }

            // Store session information
            HttpContext.Session.SetString("UserType", "Admin");
            HttpContext.Session.SetInt32("AdminId", admin.Id);

            return RedirectToAction("AdminHome", "Home");
        }

        // GET: /admin/logout
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ----------- CAR MANAGEMENT -----------

        // GET: /admin/manage-cars
        [HttpGet("manage-cars")]
        public async Task<IActionResult> ManageCars()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            var cars = await _carApi.GetAllAsync();
            return View(cars);
        }

        // GET: /admin/add-car
        [HttpGet("add-car")]
        public IActionResult AddCar()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            return View();
        }

        // POST: /admin/add-car
        [HttpPost("add-car")]
        public async Task<IActionResult> AddCar(Car car)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
                return View(car);

            await _carApi.CreateAsync(car);
            return RedirectToAction("ManageCars");
        }

        // GET: /admin/update-car/{id}
        [HttpGet("update-car/{id}")]
        public async Task<IActionResult> UpdateCar(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            var car = await _carApi.GetByIdAsync(id);
            if (car == null)
                return NotFound();

            return View("UpdateCar", car);
        }

        // POST: /admin/update-car/{id}
        [HttpPost("update-car/{id}")]
        public async Task<IActionResult> UpdateCar(int id, Car car)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
                return View(car);

            await _carApi.UpdateAsync(id, car);
            return RedirectToAction("ManageCars");
        }

        // GET: /admin/delete-car/{id}
        [HttpGet("delete-car/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            await _carApi.DeleteAsync(id);
            return RedirectToAction("ManageCars");
        }

        // ----------- CUSTOMER MANAGEMENT -----------

        // GET: /admin/manage-customers
        [HttpGet("manage-customers")]
        public async Task<IActionResult> ManageCustomers()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            var customers = await _customerApi.GetAllAsync();
            return View("ManageCustomers", customers);
        }

        // POST: Delete Customer
        [HttpPost("delete-customer")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            await _customerApi.DeleteAsync(id);
            return RedirectToAction("ManageCustomers");
        }

        // ----------- BOOKING MANAGEMENT -----------

        // GET: /admin/manage-bookings
        [HttpGet("manage-bookings")]
        public async Task<IActionResult> ManageBookings()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            var bookings = await _bookingApi.GetAllAsync();
            return View("ManageBookings", bookings);
        }

        // POST: Delete Booking
        [HttpPost("delete-booking")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            await _bookingApi.DeleteAsync(id);
            return RedirectToAction("ManageBookings");
        }
    }
}
