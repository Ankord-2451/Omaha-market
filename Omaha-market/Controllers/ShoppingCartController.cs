﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var session = new SessionWorker(HttpContext);
            if(session.IsAuthorized())
            {               
            return View(Helper.TakeProductsInCart(session,db));
            }
            return View("View");
        }


        [HttpGet("Favorite")]
        public ActionResult Favorite()
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsAuthorized())
            {
                return View(Helper.TakeFavoriteProducts(session, db));
            }
            return View("View");
        }
    }
}
