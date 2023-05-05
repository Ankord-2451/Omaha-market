using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omaha_market.Data;
using Omaha_market.Models;

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
        [HttpPost("pay/buyAll")]
        public ActionResult BuyAll(List<CartHelperModel> list)
        {
            List<int> lis= new List<int>();
            foreach (CartHelperModel item in list)
            {
                lis.Add(item.IdOfProduct);
            }
            return View("View",lis);
        }


    }
}
