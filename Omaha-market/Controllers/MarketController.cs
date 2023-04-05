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
        [HttpGet("Market/{id?}")]
        public ActionResult Index(int id = 1)
        {
            var session = new SessionWorker(HttpContext);
            ViewData["IsAdmin"] = session.IsAdmin();
            return View("Market",
                    Helper.PageSplitHelper(
                    Helper.TakeProductsOnDiscount(db),
                    id)
                );
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
                if (photo is null)
                {
                    product.Img = "Images\\NoImg.png";
                }
                else 
                {
                    using (var fileStream = new FileStream($"{configuration["Path:Image"]}{photo.FileName}", FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                    product.Img = $"Images\\{photo.FileName}";
                }
               
                product.DateOfLastChange = DateTime.Now;
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
                db.SaveChangesAsync();
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
