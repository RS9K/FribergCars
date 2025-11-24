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

        // MAIN START PAGE
        public IActionResult Index()
        {
            var userType = HttpContext.Session.GetString("UserType");

            if (userType == "Customer")
                return RedirectToAction(nameof(CustomerHome));

            if (userType == "Admin")
                return RedirectToAction(nameof(AdminHome));

            return View(); // Views/Home/Index.cshtml for guests
        }

        // CUSTOMER HOME
        public IActionResult CustomerHome()
        {
            if (HttpContext.Session.GetString("UserType") != "Customer")
            {
                return RedirectToAction("Login", "Customer");
            }

            return View(); // Views/Home/CustomerHome.cshtml
        }

        // ADMIN HOME
        public IActionResult AdminHome()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }

            return View(); // Views/Home/AdminHome.cshtml
        }

        // DEFAULT ERROR PAGE
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}


//using System.Diagnostics;
//using FribergCars.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace FribergCars.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult CustomerHome()
//        {
//            return View(); // Views/Home/CustomerHome.cshtml
//        }

//        public IActionResult AdminHome()
//        {
//            return View(); // Views/Home/AdminHome.cshtml
//        }
//        public IActionResult Index()
//        {
//            var userType = HttpContext.Session.GetString("UserType");

//            if (userType == "Admin")
//                return View("AdminHome");

//            if (userType == "Customer")
//                return View("CustomerHome");

//            return View("Index");
//        }


//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}
