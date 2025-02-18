using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bulky.Models.Models;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.Areas.Customer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _repo;

    public HomeController(ILogger<HomeController> logger , IProductRepository repository)
    {
        _logger = logger;
        _repo = repository;
    }

    public IActionResult Index()
    {
        var products = _repo.GetAll("Category");
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Details(int id)
    {
        var product = _repo.Get(id,"Category");
        return View(product);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}