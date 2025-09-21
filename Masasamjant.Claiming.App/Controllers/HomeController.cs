using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Masasamjant.Claiming.App.Models;

namespace Masasamjant.Claiming.App.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly IClaimManager claimManager;

    public HomeController(ILogger<HomeController> logger, IClaimManagerFactory claimManagerFactory, IConfiguration configuration)
    {
        this.logger = logger;
        this.claimManager = claimManagerFactory.CreateClaimManager(configuration);
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Administration()
    {
        var claims = (await claimManager.GetClaimsAsync()).ToList();
        return View(claims);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
