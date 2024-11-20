using DemoApp.Core;
using DemoApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IService<Guid, Product> service;

        public ProductController(IService<Guid, Product> service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var products = await service.ListAsync();
            return View(new ProductsViewModel(products));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await service.FindAsync(id);

            if (product == null)
                return RedirectToAction("Index", "Product");

            return View(new ProductViewModel() 
            {
                Identifier = product.Identifier,
                Name = product.Name,
                Description = product.Description
            });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new ProductViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(Guid id)
        {
            var product = await service.FindAsync(id);

            if (product == null)
                return RedirectToAction("Index", "Product");

            var claimDescriptor = await service.TryClaimAsync(product, GetSessionUser());

            if (claimDescriptor.Claim == null || claimDescriptor.IsNotFound)
                return RedirectToAction("Index", "Product");

            var viewModel = new ProductViewModel()
            {
                Identifier = product.Identifier,
                Name = product.Name,
                Description = product.Description
            };

            viewModel.ClaimDescriptor.Result = claimDescriptor.Result;
            viewModel.ClaimDescriptor.Claim = claimDescriptor.Claim;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = new Product(Guid.NewGuid(), model.Name)
            {
                Description = model.Description
            };

            await service.AddAsync(product);

            return RedirectToAction("Details", "Product", new { id = product.Identifier });
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = await service.FindAsync(model.Identifier);

            if (product == null)
                return RedirectToAction("Index", "Product");

            var claimDescriptor = await service.TryClaimAsync(product, GetSessionUser());

            if (claimDescriptor.Claim == null || claimDescriptor.IsNotFound)
                return RedirectToAction("Index", "Product");

            if (!claimDescriptor.IsDenied)
            {
                model.ClaimDescriptor.Result = claimDescriptor.Result;
                model.ClaimDescriptor.Claim = claimDescriptor.Claim;
                return View(model);
            }

            product.Name = model.Name;
            product.Description = model.Description;

            await service.ReleaseClaimAsync(claimDescriptor);

            return RedirectToAction("Details", "Product", new { id = product.Identifier });
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
