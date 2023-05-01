using Microsoft.AspNetCore.Mvc;
using Omaha_market.Core;
using Omaha_market.Data;
using Omaha_market.Models;

namespace Omaha_market.Controllers
{
    public class AdminController : Controller
    {
        private AppDbContext db;
       public AdminController(AppDbContext db)
        { 
         this.db = db;
        }
      
        [HttpGet("Product/Create")]
        public ActionResult Create()
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin()) {
                ViewData["CategoryModel"] = db.Category.ToList();
            return View("AddProduct");
            }
            return StatusCode(401);
        }

        
        [HttpPost("Product/Create")]
        public ActionResult Create(ProductModel product, IFormFile photo)
        {
            var helper = new Helper();
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                product = helper.PreparationForSaveProduct(product, db, photo);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return StatusCode(401);
        }

        
        [HttpGet("Product/Edit/{id?}")]
        public ActionResult Edit(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                ViewData["CategoryModel"]=db.Category.ToList();
                return View("EditProduct", db.Products.First(x => x.Id == id) );
            }
            return StatusCode(401);
        }

       
        [HttpPost("Product/Edit/{id?}")]
        public ActionResult Edit(int id,ProductModel product, IFormFile photo)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                var helper = new Helper();
                helper.UpdateProduct(product, db, photo);
                return RedirectToRoute("Details",product.Id);
            }
            return StatusCode(401);
        }


        [HttpPost("Product/Delete")]
        public ActionResult Delete(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                var product = db.Products.First(x => x.Id == id);
                string Path = $"wwwroot\\Images\\{product.Img}";

                System.IO.File.Delete(Path);

                db.Products.Remove(product);
                return RedirectToAction("Index");
            }
            return StatusCode(401);
        }
    }
}
