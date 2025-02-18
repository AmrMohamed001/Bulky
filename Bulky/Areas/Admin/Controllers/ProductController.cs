using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        #region Props
        private IProductRepository _repo;
        private IWebHostEnvironment _hostingEnvironment;
        private ICategoryRepository _Catrepo;
        #endregion
        public ProductController(IProductRepository repository , ICategoryRepository categoryRepository, IWebHostEnvironment webHost)
        {
            _repo = repository;
            _Catrepo = categoryRepository;
            _hostingEnvironment = webHost;
        }

        public IActionResult Index()
        {
            var products = _repo.GetAll("Category");
            return View(products);
        }

        #region Upsert
        public IActionResult upsert(int? id) 
        {
            IEnumerable<SelectListItem> selectLists = _Catrepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            ViewBag.CategoryList = selectLists;
            
            if(id == null || id == 0)
            {
                Product model = new Product();
                return View(model);
            }
            else
            {
                var product = _repo.Get(id.GetValueOrDefault());
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Product product,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                // saving file 
                string wwwRootPath = _hostingEnvironment.WebRootPath;
                if(file != null)
                {
                    // set image name
                    var image = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    // set product path
                    var path = Path.Combine(wwwRootPath, @"images\product", image);
                    // delete old image (update)
                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }
                    // streaming to the wwwroot
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    // save to db
                    product.ImageUrl = @"images\product\"+image;
                }
                if (int.TryParse(product.Title, out _))
                {
                    ModelState.AddModelError("Name", "Name must be string");
                    return View(product);
                }
                var message = "";
                if (product.Id == 0)
                {
                    _repo.Add(product);
                    message = "Product has been added successfully";
                }
                else {
                    _repo.Update(product);
                    message = "Product has been updated successfully";
                } 
                _repo.Save();
                TempData["Success"] = message;
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<SelectListItem> selectLists = _Catrepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            ViewBag.CategoryList = selectLists;
            return View(product);
        }
        #endregion

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _repo.GetAll("Category");
            return Json(new { data = products });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _repo.Get(id.GetValueOrDefault());
            if (productToBeDeleted == null) return Json(new {success = false , message = "Error while deleting"});
            var oldProductImagePath = Path.Combine(_hostingEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if(System.IO.File.Exists(oldProductImagePath)) System.IO.File.Delete(oldProductImagePath);
            _repo.Remove(productToBeDeleted);
            _repo.Save();
            return Json(new {success = true ,message = "Delete successful" });
        }
        #endregion

    }
}
