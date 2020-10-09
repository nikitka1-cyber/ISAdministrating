using MarininCars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MarininCars.Controllers
{
    public class AccountController : Controller
    {
        AccountContext db = new AccountContext();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginM Lmodel)
        {
            BdUsers User = new BdUsers();
            if (ModelState.IsValid)
            {
                User = db.BdUsers.FirstOrDefault(m => m.Login == Lmodel.Login && m.Password == Lmodel.Password);
                if (User != null)
                {
                    FormsAuthentication.SetAuthCookie(Lmodel.Login, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Пользователя с таким логином нет");

            }
            return View(Lmodel);
        }
        public ActionResult Register()
        {
            string[] role = { "a", "d", "d" };
            int[] idrole = { 1, 2, 3 };
            SelectList Role = new SelectList(role,idrole);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterM Rmodel)
        {
            BdUsers User = new BdUsers();
            if (ModelState.IsValid)
            {
                User = db.BdUsers.FirstOrDefault(m=>m.Login==Rmodel.Login);
                if (User == null)
                {
                    User = new BdUsers();
                    if (db.BdUsers.Count() > 0)
                        User.Id = db.BdUsers.Max(m => m.Id) + 1;
                    else
                        User.Id = 1;
                    User.Login = Rmodel.Login;
                    User.Password = Rmodel.Password;
                    User.Phone = Rmodel.Phone;
                    User.Email = Rmodel.Email;
                    User.RoleId = Rmodel.IdRole;
                    db.BdUsers.Add(User);
                    db.SaveChanges();
                    RedirectToAction("Account","Login");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином существует");
                }
            }
            return View();
        }
    }
}