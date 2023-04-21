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
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Account/Registration")]
        public ActionResult Regist(AccountModel account)
        {
            account.Role = role.Customer;
            account.Password = Encoder.Encode(configuration, account.Password);
            db.Accounts.Add(account);
            db.SaveChanges();

            return RedirectToAction(nameof(Index), "Authorization");
        }
        [HttpGet("Account/Edit/{id?}")]
        public ActionResult AccountEdit(int id)
        {
            try
            {
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
