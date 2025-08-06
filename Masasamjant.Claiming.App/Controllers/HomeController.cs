using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Masasamjant.Claiming.App.Models;

namespace Masasamjant.Claiming.App.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly IClaimManagerFactory claimManagerFactory;

    public HomeController(ILogger<HomeController> logger, IClaimManagerFactory claimManagerFactory)
    {
        this.logger = logger;
        this.claimManagerFactory = claimManagerFactory;
    }

    public IActionResult Index()
    {
        return View();
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
