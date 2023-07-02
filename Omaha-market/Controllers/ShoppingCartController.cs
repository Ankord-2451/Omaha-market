using Microsoft.AspNetCore.Mvc;
using Omaha_market.Core;
using Omaha_market.Data;
using Omaha_market.Models;

namespace Omaha_market.Controllers
{
    public class ShoppingCartController : Controller
    {
        AppDbContext db;

        public ShoppingCartController(AppDbContext _db) 
        {
        db= _db;    
        }


        [HttpGet("ShoppingCart")]
        public ActionResult Index()
        {
            var helper = new Helper();
            var session = new SessionWorker(HttpContext);
            ViewBag.Lang = session.GetLangDic();
            ViewData["Language"] = session.GetLanguage();
            var returnP = new Dictionary<string, string>();
            returnP.Add("act", "Index");
            returnP.Add("con", "ShoppingCart");

            ViewBag.returnP = returnP;

            if (session.IsAuthorized())
            {
                var products = helper.TakeProductsInCart(session, db);
                double sum = 0;
                foreach(var product in products)
                {
                  sum +=(product.Price*product.Quantity);
                }
                ViewData["Sum"] = sum;
            return View(products);
            }
            return View("View");
        }

        [HttpPost]
        public ActionResult ProdInOrder(List<CartHelperModel> Prod)            
        {
            var session = new SessionWorker(HttpContext);
            session.SaveOrderProd(Prod);

            return RedirectToAction("BuyAll", "Pay");
        }

        [HttpGet("Favorite")]
        public ActionResult Favorite()
        {
            var helper = new Helper();
            var session = new SessionWorker(HttpContext);
            ViewBag.Lang = session.GetLangDic();
            ViewData["Language"] = session.GetLanguage();
            var returnP = new Dictionary<string, string>();
            returnP.Add("act", "Favorite");
            returnP.Add("con", "ShoppingCart");

            ViewBag.returnP = returnP;

            if (session.IsAuthorized())
            {
                return View(helper.TakeFavoriteProducts(session, db));
            }
            return View("View");
        }

        
        public ActionResult DeleteSh(int id)
        {
            var session = new SessionWorker(HttpContext);
           
            db.ShoppingCart.Remove(db.ShoppingCart.First(x => x.IdOfProduct == id && x.IdOfCustomer == session.GetUserId()));
            db.SaveChanges();
            
            return RedirectToAction("Index");
        }

      
        public ActionResult DeleteFa(int id)
        {
            var session=new SessionWorker(HttpContext);
          
                db.favorite.Remove(db.favorite.First(x => x.IdOfProduct == id && x.IdOfCustomer == session.GetUserId()));
                db.SaveChanges();

            return RedirectToAction("Favorite");
        }

        public ActionResult Quantityplus(int id)
        {
            var session = new SessionWorker(HttpContext);
            if(db.Products.First(x=>x.Id==id).amount > db.ShoppingCart.First(x => x.IdOfProduct == id && x.IdOfCustomer == session.GetUserId()).Quantity)
            {   
               db.ShoppingCart.First(x => x.IdOfProduct == id && x.IdOfCustomer == session.GetUserId()).Quantity++;

               db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Quantityminus(int id)
        {
            var session = new SessionWorker(HttpContext);
            if(db.ShoppingCart.First(x => x.IdOfProduct == id && x.IdOfCustomer == session.GetUserId()).Quantity >1)
            { 
            db.ShoppingCart.First(x => x.IdOfProduct == id && x.IdOfCustomer == session.GetUserId()).Quantity--;
            db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
