using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omaha_market.Core;
using Omaha_market.Data;
using Omaha_market.Migrations;
using Omaha_market.Models;
using System.Text.Json;

namespace Omaha_market.Controllers
{
    [Authorize]  
    public class PayController : Controller
    {
        private AppDbContext db;
        public PayController(AppDbContext db)
        {
            this.db = db;
        }
        [HttpGet("pay/buy/{id}")]
        public ActionResult Buy(int id)
        {
            return View("View");
        }
        [HttpGet("pay/buyAll")]
        public ActionResult BuyAll()
        {
            var session = new SessionWorker(HttpContext);
           
            string IANAQOP;
            var list = session.GetOrderProd(out IANAQOP);
            ViewData["IdAndNameAndQuantityOfProduct"] = IANAQOP;

            if (list.Count == 0) 
                return RedirectToAction("Index", "Market");

            ViewBag.Lang = session.GetLangDic();
            ViewData["Language"] = session.GetLanguage();
            var returnP = new Dictionary<string, string>();
            returnP.Add("act", "BuyAll");
            returnP.Add("con", "Pay");
            ViewBag.returnP = returnP;
            ViewData["IsRu"] = session.IsRu();

            ViewData["Order"] = list;

            int sum = 0;
            foreach (var product in list)
            {
                sum += (int)(product.Price * product.Quantity);
            }
            ViewData["Sum"] = sum;

            var helper = new Helper();
            ViewBag.HelpOrderForm = helper.OrderFormHelp(db, session);

            return View("OrderForm");
        }

        [HttpPost("pay/buyAll")]
        public ActionResult BuyAll(OrderModel order) 
        {
            if (ModelState.IsValid)
            {
                var session = new SessionWorker(HttpContext);

                if (order.PaymentMethod == "Cash")
                {
                    ViewData["IsRu"] = session.IsRu();
                    db.Orders.Add(order);
                    db.SaveChanges();
                    return View("OrderIsProcessed");
                }
                else
                {                   
                    session.SaveOrder(order);
                    return RedirectToAction("CardPay");
                }
            }
            return BuyAll();
        }

        [HttpGet]
        public ActionResult CardPay()
        {
            return View("CardPayForm");
        }

        [HttpPost]
        public ActionResult CardPay(int i)
        {
            if(ModelState.IsValid) { 
            var session = new SessionWorker(HttpContext);
            var order = session.GetOrder();
            ViewData["IsRu"] = session.IsRu();
            db.Orders.Add(order);
            db.SaveChanges();
            return View("OrderIsProcessed");
              return View("Fail");
            }
            return CardPay();
        }
        public ActionResult DeleteProdFromOrder(int id)
        {
            var session = new SessionWorker(HttpContext);

            var list = session.GetOrderProd(out var str);
            list.Remove(list.First(x=>x.IdOfProduct==id));
            session.SaveOrderProd(list);

            return RedirectToAction("BuyAll");
        }
    }
}
