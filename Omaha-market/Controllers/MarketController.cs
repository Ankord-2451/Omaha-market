using Microsoft.AspNetCore.Mvc;
using Omaha_market.Core;
using Omaha_market.Data;
using Omaha_market.Models;

namespace Omaha_market.Controllers
{

    public class MarketController : Controller
    {
        private AppDbContext db;
        private IConfiguration configuration;
        public MarketController(AppDbContext _db, IConfiguration _configuration) 
        {
            db = _db;
            configuration = _configuration;          
        }


        
        [HttpGet("Market")]
        public ActionResult Index()
        {
            var helper = new Helper();
            ViewData["OnDiscount"] =helper.TakeProductsOnDiscount(db) ;

            ViewData["New"] = helper.TakeNewProducts(db);

            ViewData["Some"] = helper.TakeProductsSome(db);

            return View("Market");
        }

       
        [HttpGet("Market/Details/{id}")]
        public ActionResult Details(int id)
        {

            var session = new SessionWorker(HttpContext);
            ViewData["IsAdmin"] = session.IsAdmin();

            ViewData["Same"] = db.Products.Where(x => x.CategoryRo == db.Products.First(x => x.Id == id).CategoryRo).ToList();

            ViewData["IsRu"] = session.IsRu();

            return View( db.Products.First(x=>x.Id==id) ); 
        }

       
        [HttpGet("Category/{Name?}")]
        public ActionResult Category(string Name, int page = 1)
        {
            var helper = new Helper();
            int AmountOfPages;
            if (page <= 0) page = 1;

            var products = helper.PageSplitHelper(db.Products.Where(x=>(x.CategoryRu==Name||x.CategoryRo == Name)).ToList(), page, out AmountOfPages);

            if (page > AmountOfPages)
            {
                page = 1;
                products = helper.PageSplitHelper(db.Products.Where(x => (x.CategoryRu == Name || x.CategoryRo == Name)).ToList(), page, out AmountOfPages);
            }

            ViewData["Page"] = page;

            ViewData["AmountOfPages"] = AmountOfPages;


            return View(products);
        }
        
      
        
        [HttpGet("OnDiscount")]
        public ActionResult OnDiscount(int page = 1)
        {
            var helper = new Helper();
            ViewData["action"] = "OnDiscount";
            int AmountOfPages;
            if (page <= 0) page = 1;

            var products = helper.PageSplitHelper(helper.TakeProductsOnDiscountAll(db), page, out AmountOfPages);

            if (page > AmountOfPages)
            {
                page = 1;
                products = helper.PageSplitHelper(helper.TakeProductsOnDiscountAll(db), page, out AmountOfPages);
            }

            ViewData["Page"] = page;

            ViewData["AmountOfPages"] = AmountOfPages;


            return View("NewAndOnDiscount", products);
        }

        
        [HttpGet("New")]
        public ActionResult New(int page = 1)
        {
            var helper = new Helper();
            ViewData["action"] = "New";
            int AmountOfPages;
            if (page <= 0) page = 1;

            var products = helper.PageSplitHelper(helper.TakeNewProductsAll(db), page, out AmountOfPages);

            if (page > AmountOfPages)
            {
                page = 1;
                products = helper.PageSplitHelper(helper.TakeNewProductsAll(db), page, out AmountOfPages);
            }

            ViewData["Page"] = page;

            ViewData["AmountOfPages"] = AmountOfPages;


            return View("NewAndOnDiscount", products);
        }

        
        [HttpGet("Some")]
        public ActionResult Some(int page = 1)
        {
            var helper = new Helper();
            ViewData["action"] = "Some";
            int AmountOfPages;
            if (page <= 0) page = 1;

            var products = helper.PageSplitHelper(helper.TakeProductsSomeAll(db), page, out AmountOfPages);

            if (page > AmountOfPages)
            {
                page = 1;
                products = helper.PageSplitHelper(helper.TakeProductsSomeAll(db), page, out AmountOfPages);
            }

            ViewData["Page"] = page;

            ViewData["AmountOfPages"] = AmountOfPages;


            return View("NewAndOnDiscount", products);
        }

       
        [HttpPost]
        public ActionResult AddEmail(string Email)
        {
            db.Email.Add(new EmailModel() { Email = Email});
            db.SaveChanges();
            ViewData["Mailmessage"] = "Спасибо за вашу почту";
            return View("Market");
        }

        public ActionResult AddInShopingCart(int id)
        {
            var session = new SessionWorker(HttpContext);
            if(session.IsAuthorized())
            { 
                db.ShoppingCart.Add(new CartModel{IdOfProduct=id,IdOfCustomer=session.GetUserId()});
                db.SaveChanges();
            return RedirectToAction("Index", "ShoppingCart");
            }
            return RedirectToAction("Index", "ShoppingCart");
        }
        public ActionResult AddInFavorite(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAuthorized())
            {
                db.favorite.Add(new favoriteModel { IdOfProduct = id, IdOfCustomer = session.GetUserId() });
                db.SaveChanges();
                return RedirectToAction("Favorite", "ShoppingCart");
            }
            return RedirectToAction("Favorite", "ShoppingCart");
        }
    }
}
