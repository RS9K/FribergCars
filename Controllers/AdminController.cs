using FribergCars.Models;
using FribergCars.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FribergCars.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IAdministratorRepository _adminRepo;
        private readonly ICarRepository _carRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IBookingRepository _bookingRepo;

        public AdminController(IAdministratorRepository adminRepo, ICarRepository carRepo, ICustomerRepository customerRepo, IBookingRepository bookingRepo)
        {
            _adminRepo = adminRepo;
            _carRepo = carRepo;
            _customerRepo = customerRepo;
            _bookingRepo = bookingRepo;
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
            return View(); // Views/Admin/Login.cshtml
        }

        // POST: /admin/login
        [HttpPost("login")]
        public IActionResult Login(string email, string password)
        {
            var admin = _adminRepo.GetByEmailAndPassword(email, password);
            if (admin == null)
            {
                ViewData["Error"] = "Invalid email or password.";
                return View();
            }

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

        // GET: /admin/cars
        [HttpGet("manage-cars")]
        public IActionResult ManageCars()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            var cars = _carRepo.GetAll();
            return View(cars); // Views/Admin/ManageCars.cshtml
        }

        // GET: /admin/add-car
        [HttpGet("add-car")]
        public IActionResult AddCar()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            return View(); // Views/Admin/AddCar.cshtml
        }
        // POST: /admin/add-car
        [HttpPost("add-car")]
        public IActionResult AddCar(Car car)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
                return View(car);

            _carRepo.Add(car);
            return RedirectToAction("ManageCars");
        }

        // GET: /admin/Update-car/{id}
        [HttpGet("update-car/{id}")]
        public IActionResult UpdateCar(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            var car = _carRepo.GetById(id);
            if (car == null)
                return NotFound();

            return View("UpdateCar", car); // Views/Admin/UpdateCar.cshtml
        }
        // POST: /admin/update-car/{id}
        [HttpPost("update-car/{id}")]
        public IActionResult UpdateCar(int id, Car car)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
                return View(car);

            _carRepo.Update(car);
            return RedirectToAction("ManageCars");
        }

        // GET: /admin/delete-car/{id}
        [HttpGet("delete-car/{id}")]
        public IActionResult DeleteCar(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            var car = _carRepo.GetById(id);
            if (car == null)
                return NotFound();

            _carRepo.Delete(id);
            return RedirectToAction("ManageCars");
        }
        // GET: /admin/customers
        [HttpGet("manage-customers")]
        public IActionResult ManageCustomers()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            var customers = _customerRepo.GetAll();
            return View("ManageCustomers", customers); // Views/Admin/ManageCustomers.cshtml
        }

        // POST: Delete Customer
        [HttpPost]
        public IActionResult DeleteCustomer(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            var customer = _customerRepo.GetById(id);
            if (customer != null)
            {
                _customerRepo.Delete(id);
            }

            return RedirectToAction("ManageCustomers");
        }

        // GET: /admin/manage-bookings
        [HttpGet("manage-bookings")]
        public IActionResult ManageBookings()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            var bookings = _bookingRepo.GetAll(); 
            return View("ManageBookings", bookings);
        }

        [HttpPost]
        public IActionResult DeleteBooking(int id)
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
                return RedirectToAction("Login");

            _bookingRepo.Delete(id);
            return RedirectToAction("ManageBookings");
        }


    }
}
