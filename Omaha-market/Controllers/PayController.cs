using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omaha_market.Data;

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

       
        
    }
}
