using FIT5032_IbrahimFinalProject.Data;
using FIT5032_IbrahimFinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

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

            ViewData["TotalReviews"] = 0;
            if (TempData["UserExists"] != null)
            {
                ViewData["UserExists"] = TempData["UserExists"];
            }

            if (_context.Ratings.Select(r => r.RatingScore).Any())
            {
                var rate = _context.Ratings.Select(r => r.RatingScore);
                if (rate.Count() == 1)
                {
                    var singleRate = _context.Ratings.FirstOrDefault();
                    ViewData["TotalReviews"] = 1;
                    ViewData["AggregateRating"] = singleRate.RatingScore;
                } 
                else
                {
                    var rating = _context.Ratings.Select(r => r.RatingScore);
                    double aggregateRating = rating.Average();
                    int count = _context.Ratings.Select(r => r.RatingScore).Count();
                    ViewBag.RatingScore = aggregateRating;
                    ViewBag.TotalReview = count;
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //async public IActionResult Create(string From, string Subject, string Msg)
        //{
        //    string userName = User.Identity.Name;
        //    var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress(From, userName);
        //    var subject = Subject;
        //    // Hardcoded in, as the to email will always be sent to us (the clinic)
        //    var to = new EmailAddress("ibyster824@gmail.com", "uMRI Staff");
        //    var plainTextContent = Msg;
        //    var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        RedirectToAction(nameof(Index));
        //    } else {
        //        return View();
        //    } 
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}