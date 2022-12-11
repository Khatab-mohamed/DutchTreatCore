using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreatCore.Data.Entities;
using DutchTreatCore.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreatCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _signInManager;

        public AccountController(ILogger<AccountController> logger,SignInManager<StoreUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "App");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View();
            // The Actual SignIn
            var result = await     
                _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (!result.Succeeded) return View();
            if (Request.Query.Keys.Contains("ReturnUrl"))
                Redirect(Request.Query["ReturnUrl"].First());
            RedirectToAction("Shop", "App");

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
           await   _signInManager.SignOutAsync();
           return RedirectToAction("Index", "App");
        }
        
    }
}
