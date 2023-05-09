using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omaha_market.Core;
using Omaha_market.Data;
using Omaha_market.Models;

namespace Omaha_market.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private AppDbContext db;
        private IConfiguration configuration;
        public UsersController(AppDbContext db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
        }

        [HttpGet("Account/{id?}")]
        public ActionResult Index(int id)
        {
            try
            {
                var session = new SessionWorker(HttpContext);
                ViewBag.Lang = session.GetLangDic();
                ViewData["Language"] = session.GetLanguage();
                var returnP = new Dictionary<string, string>();
                returnP.Add("act", "Index");
                returnP.Add("con", "Users");
                returnP.Add("id", $"{id}");
                ViewBag.returnP = returnP;

                ViewData["IsAdmin"] = session.IsAdmin();
                var account = db.Accounts.First(x => x.ID == id);
                return View(account);
            }
            catch
            {
                return StatusCode(404);
            }
        }
        [AllowAnonymous]
        [HttpGet("Account/Registration")]
        public ActionResult Regist()
        {
            var session = new SessionWorker(HttpContext);
            ViewBag.Lang = session.GetLangDic();
            ViewData["Language"] = session.GetLanguage();
            var returnP = new Dictionary<string, string>();
            returnP.Add("act", "Regist");
            returnP.Add("con", "Users");
            ViewBag.returnP = returnP;

            return View();
        }

        [AllowAnonymous]
        [HttpPost("Account/Registration")]
        public ActionResult Regist(AccountModel account)
        {
            account.Role = role.Customer;
            account.Password = Encoder.Encode(configuration, account.Password);
            db.Accounts.Add(account);
            db.Email.Add(new EmailModel(){Email = account.Email });
            db.SaveChanges();

            return RedirectToAction(nameof(Index), "Authorization");
        }
        [HttpGet("Account/Edit/{id?}")]
        public ActionResult AccountEdit(int id)
        {
            try
            {
                var session = new SessionWorker(HttpContext);
                ViewBag.Lang = session.GetLangDic();
                ViewData["Language"] = session.GetLanguage();
                var returnP = new Dictionary<string, string>();
                returnP.Add("act", "AccountEdit");
                returnP.Add("con", "Users");
                ViewBag.returnP = returnP;

                return View("Edit",db.Accounts.First(x => x.ID == id));
            }
            catch
            {
                return StatusCode(404);
            }
        }

        [HttpPost("Account/Edit/{id?}")]
        public ActionResult AccountEdit(AccountModel account)
        {
            account.Password = db.Accounts.First(x => x.ID == account.ID).Password;
            db.Accounts.Remove(db.Accounts.First(x => x.ID == account.ID));
            db.Accounts.Add(account);
            db.SaveChanges();
            return RedirectToAction("Index", "Market");
        }

        [HttpPost("Account/Delete/{id?}")]
        public ActionResult Delete(int id)
        {
            try
            {
                db.Accounts.Remove(db.Accounts.First(x => x.ID == id));
                db.ShoppingCart.RemoveRange(db.ShoppingCart.Where(x => x.IdOfCustomer == id));
                db.favorite.RemoveRange(db.favorite.Where(x => x.IdOfCustomer == id));
                db.SaveChanges();
                return RedirectToAction("LogOut", "Authorization");
            }
            catch
            {
                return StatusCode(404);
            }
        }
    }
}
