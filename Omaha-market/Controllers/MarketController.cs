using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Index(int page = 1)
        {
            if (page <= 0) page = 1;

            int AmountOfPages;

            var products = Helper.PageSplitHelper(db.Products.ToList(), page, out AmountOfPages);

            if (page > AmountOfPages)
            {
                page = 1;
                products = Helper.PageSplitHelper(db.Products.ToList(), page, out AmountOfPages);
            }

            ViewData["Page"] = page;

            ViewData["AmountOfPages"] = AmountOfPages;

            ViewData["OnDiscount"] = Helper.TakeProductsOnDiscount(db);

            ViewData["ArrowBack"] = (page > 1);

            ViewData["Arrowforward"] = (page != AmountOfPages);


            return View("Market", products);
        }

        [AllowAnonymous]
        [HttpGet("Market/Details/{id?}")]
        public ActionResult Details(int id)
        {
            var session = new SessionWorker(HttpContext);
            ViewData["IsAdmin"] = session.IsAdmin();
            return View( db.Products.First(x=>x.Id==id) ); 
        }

       
        [HttpGet("Market/Create")]
        public ActionResult Create()
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin()) {
                ViewData["CategoryModel"] = db.Category.ToList();
            return View("AddProduct");
            }
            return StatusCode(401);
        }

        
        [HttpPost("Market/Create")]
        public ActionResult Create(ProductModel product, IFormFile photo)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                product.Img = Helper.SaveImg(photo);
               
                product.DateOfLastChange = DateTime.Now;
                product.CategoryRo = db.Category.FirstOrDefault(x => x.NameRu == product.CategoryRu).NameRo;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return StatusCode(401);
        }

        
        [HttpGet("Market/Edit/{id?}")]
        public ActionResult Edit(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                return View( db.Products.First(x => x.Id == id) );
            }
            return StatusCode(401);
        }

       
        [HttpPost("Market/Edit")]
        public ActionResult Edit(ProductModel product)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                product.DateOfLastChange = DateTime.Now;
                db.Products.Update(product);
                db.SaveChanges();
                return View();
            }
            return StatusCode(401);
        }


        [HttpPost("Market/Delete")]
        public ActionResult Delete(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                db.Products.Remove( db.Products.First(x => x.Id == id) );
                return RedirectToAction("Index");
            }
            return StatusCode(401);
        }
    }
}
