using System.Diagnostics;
using FribergCars.Models;
using Microsoft.AspNetCore.Mvc;

namespace FribergCars.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult CustomerHome()
        {
            return View(); // Views/Home/CustomerHome.cshtml
        }

        public IActionResult AdminHome()
        {
            return View(); // Views/Home/AdminHome.cshtml
        }
        public IActionResult Index()
        {
            var userType = HttpContext.Session.GetString("UserType");

            if (userType == "Admin")
                return View("AdminHome");

            if (userType == "Customer")
                return View("CustomerHome");

            return View("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
