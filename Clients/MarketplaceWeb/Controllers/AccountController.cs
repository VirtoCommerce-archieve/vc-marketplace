using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using MarketplaceWeb.Models;
using VirtoCommerce.ApiClient.DataContracts.Security;

namespace MarketplaceWeb.Controllers
{
    [Authorize]
    public class AccountController : ControllerBase
    {

        public ActionResult Index()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Logs on.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="loginAs">Impersonate user name</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string loginAs)
        {
            var model = new LoginViewModel();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            string errorMessage = null;
            if (ModelState.IsValid)
            {

                try
                {
                    var authUser = await SecurityClient.LoginAsync(new UserLogin {Password = model.Password, RememberMe = model.RememberMe, UserName = model.Email});
                    return RedirectToLocal(returnUrl);
                }
                catch (Exception e)
                {
                    errorMessage = "The user name or password provided is incorrect.";
                }
                
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", errorMessage);
            return View(model);
        }

        public ActionResult Register()
        {
            throw new NotImplementedException();
        }

        public ActionResult Profile(string id)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Contact(string id)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Redirects to local.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Page");
        }

    }
}