using Bulky.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles =SD.Role_Admin)]
public class CategoryController : Controller
{
    private ICategoryRepository _repo;
    public CategoryController(ICategoryRepository categoryRepo)
    {
        _repo = categoryRepo;
    }

    public IActionResult Index()
    {
        var categories = _repo.GetAll();
        return View(categories);
    }

    #region Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }
        if (int.TryParse(category.Name, out _))
        {
            ModelState.AddModelError("Name", "Name must be string");
            return View(category);
        }
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "Name must Not equal order display");
            return View(category);
        }
        _repo.Add(category);
        _repo.Save();
        TempData["Success"] = "Category has been added successfully";
        return RedirectToAction("Index");
    }
    #endregion

    #region Edit
    [HttpGet]
    public IActionResult Edit(int id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var category = _repo.Get(id);
        return View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category obj)
    {
        if (!ModelState.IsValid) return View(obj);
        _repo.Update(obj);
        _repo.Save();
        TempData["Success"] = "Category has been updated successfully";
        return RedirectToAction("Index");
    }
    #endregion

    #region Delete
    public IActionResult Delete(int id)
    {
        var category = _repo.Get(id);
        if (category == null) return NotFound();
        _repo.Remove(category);
        _repo.Save();
        TempData["Success"] = "Category has been deleted successfully";
        return RedirectToAction("Index");
    }
    #endregion
}
