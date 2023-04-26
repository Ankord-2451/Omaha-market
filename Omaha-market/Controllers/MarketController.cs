using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Omaha_market.Core;
using Omaha_market.Data;
using Omaha_market.Models;

namespace Omaha_market.Controllers
{
    [Authorize]
    public class MarketController : Controller
    {
        private AppDbContext db;
        private IConfiguration configuration;
        public MarketController(AppDbContext _db, IConfiguration _configuration) 
        {
            db = _db;
            configuration = _configuration;          
        }


        [AllowAnonymous]
        [HttpGet("Market")]
        public ActionResult Index()
        {          
            ViewData["OnDiscount"] = Helper.TakeProductsOnDiscount(db);

            return View("Market", db.Products.ToList());
        }

        [AllowAnonymous]
        [HttpGet("Market/Details/{id?}")]
        public ActionResult Details(int id)
        {
            var session = new SessionWorker(HttpContext);
            ViewData["IsAdmin"] = session.IsAdmin();
            return View( db.Products.First(x=>x.Id==id) ); 
        }

        [AllowAnonymous]
        [HttpGet("Category/{Name}")]
        public ActionResult Category(string Name, int page = 1)
        {
            int AmountOfPages;
            if (page <= 0) page = 1;

            var products = Helper.PageSplitHelper(db.Products.Where(x=>(x.CategoryRu==Name||x.CategoryRo == Name)).ToList(), page, out AmountOfPages);

            if (page > AmountOfPages)
            {
                page = 1;
                products = Helper.PageSplitHelper(db.Products.Where(x => (x.CategoryRu == Name || x.CategoryRo == Name)).ToList(), page, out AmountOfPages);
            }

            ViewData["Page"] = page;

            ViewData["AmountOfPages"] = AmountOfPages;


            return View(products);
        }
        
      
        [AllowAnonymous]
        [HttpGet("Products/OnDiscount")]
        public ActionResult OnDiscount(int page = 1)
        {
            ViewData["action"] = "OnDiscount";
            int AmountOfPages;
            if (page <= 0) page = 1;

            var products = Helper.PageSplitHelper(db.Products.Where(x => x.OnDiscount).ToList(), page, out AmountOfPages);

            if (page > AmountOfPages)
            {
                page = 1;
                products = Helper.PageSplitHelper(db.Products.Where(x => x.OnDiscount).ToList(), page, out AmountOfPages);
            }

            ViewData["Page"] = page;

            ViewData["AmountOfPages"] = AmountOfPages;


            return View("NewAndOnDiscount", products);
        }

        [AllowAnonymous]
        [HttpGet("Products/New")]
        public ActionResult New(int page = 1)
        {
            ViewData["action"] = "New";
            int AmountOfPages;
            if (page <= 0) page = 1;

            var products = Helper.PageSplitHelper(db.Products.Where(x => Helper.IsNew(x.DateOfLastChange)).ToList(), page, out AmountOfPages);

            if (page > AmountOfPages)
            {
                page = 1;
                products = Helper.PageSplitHelper(db.Products.Where(x => Helper.IsNew(x.DateOfLastChange)).ToList(), page, out AmountOfPages);
            }

            ViewData["Page"] = page;

            ViewData["AmountOfPages"] = AmountOfPages;


            return View("NewAndOnDiscount", products);
        }

        [AllowAnonymous]
        [HttpGet("Products/Some")]
        public ActionResult Some(int page = 1)
        {
            ViewData["action"] = "Some";
            int AmountOfPages;
            if (page <= 0) page = 1;

            var products = Helper.PageSplitHelper(db.Products.Where(x => x.FromSome).ToList(), page, out AmountOfPages);

            if (page > AmountOfPages)
            {
                page = 1;
                products = Helper.PageSplitHelper(db.Products.Where(x => x.FromSome).ToList(), page, out AmountOfPages);
            }

            ViewData["Page"] = page;

            ViewData["AmountOfPages"] = AmountOfPages;


            return View("NewAndOnDiscount", products);
        }
        
    }
}
