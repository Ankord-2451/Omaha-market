using Microsoft.AspNetCore.Mvc;
using Omaha_market.Core;
using Omaha_market.Data;
using Omaha_market.Models;
using static System.Net.Mime.MediaTypeNames;

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
            if (session.IsAdmin())
            {
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
                ViewData["CategoryModel"] = db.Category.ToList();
                return View("EditProduct", db.Products.First(x => x.Id == id));
            }
            return StatusCode(401);
        }


        [HttpPost("Product/Edit/{id?}")]
        public ActionResult Edit(int id, ProductModel product, IFormFile photo)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                var helper = new Helper();
                helper.UpdateProduct(product, db, photo);
                return View("PWC", product.Id);
            }
            return StatusCode(401);
        }



        [HttpGet("YouSure/{id}")]
        public ActionResult WantDelete(int id)
        {
            return View("YouSure", id);
        }

        [HttpGet("Product/Delete/{id}")]
        public ActionResult Delete(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                var product = db.Products.First(x => x.Id == id);

                if (product.Img != "NoImg.png")
                    System.IO.File.Delete($"wwwroot\\Images\\{product.Img}");


                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index", "Market");
            }
            return StatusCode(401);
        }


        [HttpGet("Admin/Category")]
        public ActionResult Category()
        {
            return View(db.Category.ToList());
        }
        [HttpGet("Admin/CategoryD/{id}")]
        public ActionResult CategoryD(int id)
        {
            db.Category.Remove(db.Category.First(x => x.Id == id));
            db.SaveChanges();
            return RedirectToAction("Category");
        }
        [HttpGet("Admin/CategoryC")]
        public ActionResult CategoryC(CategoryModel category)
        {
            db.Category.Remove(db.Category.First(x => x.Id == category.Id));
            db.Category.Add(category);
            db.SaveChanges();
            return RedirectToAction("Category");
        }
        [HttpGet("Admin/AddCategory")]
        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpGet("Admin/AddCategory")]
        public ActionResult AddCategory(CategoryModel category)
        {
            db.Category.Add(category);
            return RedirectToAction("Category");
        }


        [HttpGet("Admin/Text")]
        public ActionResult ChangeText() 
        {
            return View(db.Text.FirstOrDefault());
        }
        [HttpGet("Admin/Text")]
        public ActionResult ChangeText(TextModel text)
        {
            db.Text.Remove(db.Text.FirstOrDefault());
            db.Text.Add(text);
            db.SaveChanges();
            return RedirectToAction("Index","Authorization");
        }
    }
}
