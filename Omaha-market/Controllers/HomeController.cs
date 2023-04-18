using Microsoft.AspNetCore.Mvc;
using Omaha_market.Data;
using Omaha_market.Core;

namespace Omaha_market.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext dbContext;
        public HomeController(AppDbContext appDb)
        {
            dbContext = appDb;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchstr)
        {
            return View(await Helper.FuzzySearchAsync(searchstr, dbContext.Products.ToList()));           
        }
    }
}