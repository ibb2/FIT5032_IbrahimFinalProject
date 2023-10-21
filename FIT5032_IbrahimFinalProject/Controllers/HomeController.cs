using FIT5032_IbrahimFinalProject.Data;
using FIT5032_IbrahimFinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FIT5032_IbrahimFinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ClinicContext _context;

        public HomeController(ILogger<HomeController> logger, ClinicContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            double aggregateRating = _context.Ratings.Select(r => r.RatingScore).Average();
            ViewData["AggregateRating"] = aggregateRating;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}