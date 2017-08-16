using CASMUL.DB;
using CASMUL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASMUL.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            using (var db = new dbcasmulEntities())
            {
                try
                {
                    var usuario = db.AspNetUsers.Where(x => x.Email == model.UserName).FirstOrDefault();
                    if (usuario.Email != "")
                    {
                        Session["USUARIO"] = usuario.UserName;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Contraseña Invalida");
                        return View(model);
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Contraseña Invalida");
                    return View(model);
                }

            }
        }

    }
}