﻿using Microsoft.AspNetCore.Mvc;
using Omaha_market.Core;
using Omaha_market.Data;

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
            if(session.IsAuthorized())
            {               
            return View(helper.TakeProductsInCart(session,db));
            }
            return View("View");
        }


        [HttpGet("Favorite")]
        public ActionResult Favorite()
        {
            var helper = new Helper();
            var session = new SessionWorker(HttpContext);
            if (session.IsAuthorized())
            {
                return View(helper.TakeFavoriteProducts(session, db));
            }
            return View("View");
        }
    }
}
