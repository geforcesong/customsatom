using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CustomsAtomMobileSite.Models;

namespace CustomsAtomMobileSite.Controllers
{
    public class AccountController : Controller
    {

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(CustomUser model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                CustomUser user = CustomUser.Login(model.Alias, model.Password);
                if (user != null)
                {
                    Session["LoginID"] = user.ID;
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    model.Alias,
                    DateTime.Now,
                    DateTime.Now.Add(FormsAuthentication.Timeout),
                    true,//model.RememberMe,
                    "Admin"
                    );
                    HttpCookie cookie = new HttpCookie(
                        FormsAuthentication.FormsCookieName,
                        FormsAuthentication.Encrypt(ticket));
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        
    }
}
