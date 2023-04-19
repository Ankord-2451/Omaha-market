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

        [HttpGet("Account")]
        public ActionResult Index(int id)
        {
            try
            {
                return View(db.Accounts.First(x => x.ID == id));
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
        public ActionResult Edit(int id)
        {
            try
            {
                return View(db.Accounts.First(x => x.ID == id));
            }
            catch
            {
                return StatusCode(404);
            }
        }

        [HttpPost("Account/Edit")]
        public ActionResult Edit(AccountModel account)
        {
            db.Accounts.Update(account);
            db.SaveChanges();
           return RedirectToAction(nameof(Index));
        }

        [HttpPost("Account/Delete/{id?}")]
        public ActionResult Delete(int id)
        {
            try
            {
                db.Accounts.Remove(db.Accounts.First(x => x.ID == id));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return StatusCode(404);
            }
        }
    }
}
