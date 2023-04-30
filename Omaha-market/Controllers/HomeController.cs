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

        [HttpGet]
        public async Task<IActionResult> Search(string searchstr,int page = 1)
        {
            var helper = new Helper();
            int AmountOfPages;

            if (page <= 0) page = 1;

            var products = helper.PageSplitHelper(await helper.FuzzySearchAsync(searchstr, dbContext.Products.ToList()), page, out AmountOfPages);

            if (page > AmountOfPages)
            {
                page = 1;
                products = helper.PageSplitHelper(await helper.FuzzySearchAsync(searchstr, dbContext.Products.ToList()), page, out AmountOfPages);
            }

            ViewData["Page"] = page;

            ViewData["AmountOfPages"] = AmountOfPages;

            ViewData["searchstr"] = searchstr;

            return View(products);           
        }
    }
}