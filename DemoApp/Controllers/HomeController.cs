using DemoApp.Core;
using DemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DemoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<string, User> userService;

        public HomeController(IService<string, User> userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? redirectUrl = null)
        {
            return View(new LoginViewModel() { RedirectUrl = redirectUrl });
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model) 
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userService.FindAsync(model.UserName);

            if (user == null)
            {
                ModelState.AddModelError(nameof(model.UserName), "The user does not exist.");
                return View(model);
            }

            HttpContext.Session.SetString("User", model.UserName);

            if (string.IsNullOrWhiteSpace(model.RedirectUrl))
                return RedirectToAction("Index", "Home");
            else
                return Redirect(model.RedirectUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
