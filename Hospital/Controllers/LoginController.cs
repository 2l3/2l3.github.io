using Hospital.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Hospital.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ActionResult Index()
        {
            return View();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Index(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            else
            {
                var contxt = System.Web.HttpContext.Current.GetOwinContext();

                var Authmanger = contxt.Authentication;
                var userManager = ApplicationUserManager.Create(contxt);
                var signinmanger = new ApplicationSignInManager(userManager, Authmanger);

                var result = await signinmanger.PasswordSignInAsync(model.Email, model.Password, true, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    //---------------------------------------------------------------------------
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View("Index", model);
                }
            }
        }
    }
}