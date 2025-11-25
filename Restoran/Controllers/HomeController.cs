using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Restoran.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Restoran.Controllers
{
    [Authorize] // Only logged-in users can access
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Dependency injection for logging
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Default route: /
        public IActionResult Index()
        {
            return View();
        }

        // Route: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // Handles errors and shows error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
        public IActionResult Ingredient()
        {
            return View();
        }
        //public IActionResult Test1()
        //{
        //    return View();
        //}

        //public IActionResult Test2()
        //{
        //    return View();
        //}

    }
}