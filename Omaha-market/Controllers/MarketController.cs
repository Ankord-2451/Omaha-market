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
        private SessionWorker session;
        public MarketController(AppDbContext _db) 
        {
            session = new SessionWorker(HttpContext);
            db = _db;
        }


        [AllowAnonymous]
        [HttpGet("Market/?id")]
        public ActionResult Index(int id)
        {
            ViewData["IsAdmin"] = session.IsAdmin();
            return View(
                    Helper.PageSplitHelper(
                    Helper.TakeProductsOnDiscount(db),
                    id)
                );
        }

        [AllowAnonymous]
        [HttpGet("Market/Details/?id")]
        public ActionResult Details(int id)
        {
            ViewData["IsAdmin"] = session.IsAdmin();
            return View( db.Products.First(x=>x.Id==id) ); 
        }

       
        [HttpGet("Market/Create")]
        public ActionResult Create()
        {
            if (session.IsAdmin()) { 
            return View();
            }
            return StatusCode(401);
        }

        
        [HttpPost("Market/Create")]
        public ActionResult Create(ProductModel product)
        {
            if (session.IsAdmin())
            {
                db.Products.Add(product);
                db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return StatusCode(401);
        }

        
        [HttpGet("Market/Edit/?id")]
        public ActionResult Edit(int id)
        {
            if (session.IsAdmin())
            {
                return View( db.Products.First(x => x.Id == id) );
            }
            return StatusCode(401);
        }

       
        [HttpPost("Market/Edit/?id")]
        public ActionResult Edit(ProductModel product)
        {
            if (session.IsAdmin())
            {
                db.Products.Update(product);
                db.SaveChangesAsync();
                return View();
            }
            return StatusCode(401);
        }


        [HttpPost("Market/Delete/?id")]
        public ActionResult Delete(int id)
        {
            if (session.IsAdmin())
            {
                db.Products.Remove( db.Products.First(x => x.Id == id) );
                return RedirectToAction("Index");
            }
            return StatusCode(401);
        }
    }
}
