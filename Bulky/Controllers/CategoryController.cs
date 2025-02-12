using Bulky.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Controllers;

public class CategoryController : Controller
{
    private AppDbContext db;
    public CategoryController(AppDbContext ctx)
    {
        db = ctx;
    }
    // GET
    public IActionResult Index()
    {
        var categories = db.Categories.ToList();
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
        if(category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "Name must Not equal order display");
            return View(category);
        }
        db.Categories.Add(category);
        db.SaveChanges();
        TempData["Success"] = "Category has been added successfully";
        return RedirectToAction("Index");
    }
    #endregion

    #region Edit
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if(id == null)
        {
            return NotFound();
        }
        var category = db.Categories.Find(id);
        return View(category);
    }
    [HttpPost]
    public IActionResult Edit(Category obj)
    {
        if(!ModelState.IsValid) return View(obj);
        db.Categories.Update(obj);
        db.SaveChanges();
        TempData["Success"] = "Category has been updated successfully";
        return RedirectToAction("Index");
    }
    #endregion

    #region Delete
    public IActionResult Delete(int id) {
    var category = db.Categories.Find(id);
        if (category == null) return NotFound();
        db.Categories.Remove(category);
        db.SaveChanges();
        TempData["Success"] = "Category has been deleted successfully";
        return RedirectToAction("Index");
    }
    #endregion
}
