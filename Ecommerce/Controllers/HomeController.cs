using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _dbContext;

        public HomeController(MyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var categories = _dbContext.Categories.ToList();
            ViewBag.categories = categories;
            var products = _dbContext.Products.Include(p => p.Category).ToList();
            ViewBag.products = products;

            return View(categories);
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

        [Authorize]
        public IActionResult AuthorizedPage()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult UserPage()
        {
            return View();
        }
    }
}
