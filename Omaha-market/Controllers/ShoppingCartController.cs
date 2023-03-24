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
            var session = new SessionWorker(HttpContext);
            if(session.IsAuthorized())
            {
                List<ProductModel> products = new List<ProductModel>();

                List<CartModel> IdsOfProducts = db.ShoppingCart.Where(x => x.IdOfCustomer == session.GetUserId()).ToList();
                foreach(CartModel cart in IdsOfProducts)
                {
                    products.Add(  (ProductModel)db.Products.Where(x=> x.Id==cart.IdOfProduct)  );
                }
            return View(products);
            }
            return View("View");
        }


        [HttpGet("Favorite")]
        public ActionResult Favorite()
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAuthorized())
            {
                List<ProductModel> products = new List<ProductModel>();

                List<favoriteModel> IdsOfProducts = db.favorite.Where(x => x.IdOfCustomer == session.GetUserId()).ToList();
                foreach (favoriteModel cart in IdsOfProducts)
                {
                    products.Add((ProductModel)db.Products.Where(x => x.Id == cart.IdOfProduct));
                }
                return View(products);
            }
            return View("View");
        }
    }
}
