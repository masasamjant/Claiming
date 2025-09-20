using DemoApp.Core;
using DemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IService<string, User> service;

        public UserController(IService<string, User> service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var users = await service.ListAsync();
            return View(new UsersViewModel(users));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string name, bool cancel = false)
        {
            var user = await service.FindAsync(name);

            if (user == null)
                return RedirectToAction("Index", "User");

            if (cancel)
            {
                var ownerIdentifier = GetSessionUser();
                var claim = await service.GetClaimAsync(user.GetClaimKey(), ownerIdentifier);

                if (claim != null)
                    await service.ReleaseClaimAsync(claim);
            }

            return View(new UserViewModel()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new UserViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(string name)
        {
            var user = await service.FindAsync(name);

            if (user == null)
                return RedirectToAction("Index", "User");

            // Try claim "user" to current application user.
            var claimDescriptor = await service.TryClaimAsync(user, GetSessionUser());

            if (claimDescriptor.IsEmpty || claimDescriptor.IsNotFound)
                return RedirectToAction("Index", "User");

            var viewModel = new UserViewModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ClaimDescriptor = claimDescriptor
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User(model.UserName)
            {
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            await service.AddAsync(user);
            return RedirectToAction("Details", "User", new { name = user.UserName });
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await service.FindAsync(model.UserName);

            if (user == null)
                return RedirectToAction("Index", "User");

            // Get claim
            var claimDescriptor = await service.TryClaimAsync(user, GetSessionUser());

            if (claimDescriptor.IsEmpty || claimDescriptor.IsNotFound)
                return RedirectToAction("Index", "User");

            if (!claimDescriptor.IsDenied)
            {
                model.ClaimDescriptor = claimDescriptor;
                return View(model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            // Edit done release claim
            await service.ReleaseClaimAsync(claimDescriptor);

            return RedirectToAction("Details", "User", new { name = user.UserName });
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionUser = GetSessionUser();

            if (string.IsNullOrWhiteSpace(sessionUser))
            {
                var redirectUrl = Url.Action(((ControllerActionDescriptor)context.ActionDescriptor).ActionName, "Product");
                context.Result = RedirectToAction("Login", "Home", new { redirectUrl });
            }
        }

        private string GetSessionUser()
        {
            return HttpContext.Session.GetString("User") ?? string.Empty;
        }
    }
}
