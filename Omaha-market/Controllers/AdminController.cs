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
        public ActionResult Create(ProductModel product, IFormFile photo,string Price,string PriceOnDiscount)
        {
            var helper = new Helper();
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                product.Price=double.Parse(Price);
                if(PriceOnDiscount != null)  product.PriceOnDiscount = double.Parse(PriceOnDiscount);
               
                product = helper.PreparationForSaveProduct(product, db, photo);
                db.Products.Add(product);
                try{db.SaveChanges();} 
                catch{ return Create(); }
                return RedirectToAction("Index","Users",new {id = session.GetUserId()});
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
        public ActionResult Edit(int id, ProductModel product, IFormFile photo, string Price, string PriceOnDiscount)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                product.Price = double.Parse(Price);
                if (PriceOnDiscount != null) product.PriceOnDiscount = double.Parse(PriceOnDiscount);

                var helper = new Helper();
                helper.UpdateProduct(product, db, photo);
                return View("PWC", product.Id);
            }
            return StatusCode(401);
        }



        [HttpGet("YouSure/{id}")]
        public ActionResult WantDelete(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
                return View("YouSure", id);

            return StatusCode(401);
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
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                db.Category.Remove(db.Category.First(x => x.Id == id));
                db.SaveChanges();
            }
            return RedirectToAction("Category");
        }

        [HttpGet("Admin/CategoryC/{id?}")]
        public ActionResult CategoryC(int id)
        {
            return View("EditCategory", db.Category.First(x => x.Id == id));
        }
        [HttpPost("Admin/CategoryC/{id?}")]
        public ActionResult CategoryC(CategoryModel category)
        {
             if (ModelState.IsValid)
            {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                db.Category.Remove(db.Category.First(x => x.Id == category.Id));
                db.Category.Add(category);
                db.SaveChanges();
            }
                return RedirectToAction("Category");
            }
            return CategoryC(category.Id);
            
        }

        [HttpGet("Admin/AddCategory")]
        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost("Admin/AddCategory")]
        public ActionResult AddCategory(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                db.Category.Add(category);
                db.SaveChanges();
            }
            return RedirectToAction("Category");
            }
            return AddCategory();
        }


        [HttpGet("Admin/Text")]
        public ActionResult ChangeText() 
        {
            return View(db.Text.FirstOrDefault());
        }
        [HttpPost("Admin/Text")]
        public ActionResult ChangeText(TextModel text)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin()) { 
            db.Text.Remove(db.Text.FirstOrDefault());
            db.Text.Add(text);
                try { db.SaveChanges(); }
                catch { }
            }
            return RedirectToAction("Index","Authorization");       
        }

        [HttpGet("Admin/Orders")]
        public ActionResult OrdersList()
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {            
                return View("Orders",db.Orders.OrderByDescending(x => x.Id).ToList());
            }
            return StatusCode(401);
        }
        [HttpGet("Admin/OrdersProducts/{id}")]
        public ActionResult ProductsInOrder(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                var helper= new Helper();             
                return View(helper.TakeProductsInOrder(db.Orders.First(x => x.Id == id).IdAndNameAndQuantityOfProduct));
            }
            return StatusCode(401);
        }

        [HttpGet("WantDeleteOrder/{id}")]
        public ActionResult WantDeleteOrder(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
                return View("DeleteOrder", id);

            return StatusCode(401);
        }

        [HttpGet("DeleteOrder/{id}")]
        public ActionResult DeleteOrder(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                var order = db.Orders.First(x => x.Id == id);

                db.Orders.Remove(order);
               
                db.SaveChanges();
                return RedirectToAction("OrdersList");
            }
            return StatusCode(401);
        }

        
         [HttpGet("Admin/Emails")]
        public ActionResult ShowEmails()
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAdmin())
            {
                return View("mail",db.Email.ToList());
            }
            return StatusCode(401);
        }

    }
}
