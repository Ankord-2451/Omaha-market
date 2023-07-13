using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omaha_market.Core;
using Omaha_market.Data;
using Omaha_market.Migrations;
using Omaha_market.Models;
using Stripe;
using System.Text.Json;

namespace Omaha_market.Controllers
{
   
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
            var session = new SessionWorker(HttpContext);
            if (session.GetLangDic() == null)
            {
                return RedirectToAction("Lang", "Home", new { act = "Buy", con = "Pay", id = id });
            }
            ViewBag.Lang = session.GetLangDic();
            ViewData["Language"] = session.GetLanguage();
            var returnP = new Dictionary<string, string>();
            returnP.Add("act", "Buy");
            returnP.Add("con", "Pay");
            returnP.Add("id", $"{id}");

            ViewBag.returnP = returnP;

            var OrderProd = session.GetOrderProd(out var s);
            
            if (OrderProd == null || OrderProd.Count==0|| OrderProd.First().IdOfProduct != id)
            { 
                var Prod = db.Products.First(x=>x.Id==id);

                if (Prod.OnDiscount) { 
                       OrderProd = new List<Models.CartHelperModel>()
                       {
                          new Models.CartHelperModel(){
                          IdOfProduct= id,
                          Quantity=1,
                          Img=Prod.Img,
                          NameRo=Prod.NameRo,
                          NameRu=Prod.NameRu,
                          Price=Prod.PriceOnDiscount}
                       };
                }
                else
                {
                   OrderProd = new List<Models.CartHelperModel>() 
                       {
                          new Models.CartHelperModel(){
                          IdOfProduct= id,
                          Quantity=1,
                          Img=Prod.Img,
                          NameRo=Prod.NameRo,
                          NameRu=Prod.NameRu,
                          Price=Prod.Price}
                       };
                }

                session.SaveOrderProd(OrderProd);
            }

            return View("View",OrderProd);
        }
       
        public ActionResult Quantityplus(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (db.Products.First(x => x.Id == id).amount > session.GetOrderProd(out var str).First(x => x.IdOfProduct == id).Quantity)
            {
                var OrderProd = session.GetOrderProd(out var st).First(x => x.IdOfProduct == id);
                OrderProd.Quantity++;
                session.SaveOrderProd(new List<Models.CartHelperModel>() { OrderProd });
            }
            return RedirectToAction("Buy", new { id = id });
        }
       
        public ActionResult Quantityminus(int id)
        {
            var session = new SessionWorker(HttpContext);
            if (session.GetOrderProd(out var str).First(x => x.IdOfProduct == id).Quantity > 1)
            {
               var OrderProd = session.GetOrderProd(out var st).First(x => x.IdOfProduct == id);
                OrderProd.Quantity--;
                session.SaveOrderProd(new List<Models.CartHelperModel>(){ OrderProd });
            }

            return RedirectToAction("Buy",new { id = id });
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

            if (session.GetLangDic() == null)
            {
                return RedirectToAction("Lang", "Home", new { act = "BuyAll", con = "Pay" });
            }
            ViewBag.Lang = session.GetLangDic();
            ViewData["Language"] = session.GetLanguage();
            var returnP = new Dictionary<string, string>();
            returnP.Add("act", "BuyAll");
            returnP.Add("con", "Pay");
            ViewBag.returnP = returnP;
            ViewData["IsRu"] = session.IsRu();

            ViewData["Order"] = list;

            double sum = 0;
            foreach (var product in list)
            {
                sum += (product.Price * product.Quantity);
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
                var helper = new Helper();
                var session = new SessionWorker(HttpContext);

                if (helper.CheckingTheQuantityOfGoodsInStock(db, order.IdAndNameAndQuantityOfProduct))
                {
                    if (order.PaymentMethod == "Cash")
                    {
                        ViewData["IsRu"] = session.IsRu();

                        helper.ChangingTheQuantityOfGoodsInStock(db, order.IdAndNameAndQuantityOfProduct);
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
                else
                {
                    return View("NotEnough");
                }
            }
                return BuyAll();           
        }

        [HttpGet]
        public ActionResult CardPay()
        {
            var session = new SessionWorker(HttpContext);
            if (session.IsRu()) 
            { 
                ViewData["CardButton"] = "Ввести данные карты";
            }
            else
            {
                ViewData["CardButton"] = "Introduceți detaliile cardului";
            }
            return View("CardPayForm");
        }

        [HttpPost]
        public ActionResult CardPay(string stripeEmail,string stripeToken)
        {
           var helper = new Helper();
            var session = new SessionWorker(HttpContext);
            var order = session.GetOrder();
            ViewData["IsRu"] = session.IsRu();

                var Customers = new CustomerService();
                var Charges = new ChargeService();

                var customer = Customers.Create(new CustomerCreateOptions
                {
                    Email= stripeEmail,
                    Source=stripeToken
                });

                var Charge = Charges.Create(new ChargeCreateOptions
                {
                    Currency = "mdl",
                    Amount = (long)(order.PriceOfOrder*100),
                    Customer = customer.Id,
                    Metadata = new Dictionary<string,string>()
                    {
                        {"Name :",$"{order.Name}"},
                        {"PhoneNumber :",$"{order.PhoneNumber}"}
                    }
                });

                if(Charge.Status == "succeeded")
                {
                    helper.ChangingTheQuantityOfGoodsInStock(db, order.IdAndNameAndQuantityOfProduct);
               
                    order.IdOfTransaction = Charge.BalanceTransactionId;

                    db.Orders.Add(order);
                    db.SaveChanges();

                    return View("OrderIsProcessed");
                }
                
                return View("Fail");
                        
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
