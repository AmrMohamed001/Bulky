using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Utility.SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _repo;
        public CompanyController(ICompanyRepository companyRepository)
        {
            _repo = companyRepository;
        }
        public IActionResult Index()
        {
            var companies = _repo.GetAll();
            return View(companies);
        }
        #region Upsert
        public IActionResult Upsert(int? id) {
            if (id == null || id == 0)
            {
                Company model = new Company();
                return View(model);
            }
            else
            {
                var company = _repo.Get(id.GetValueOrDefault());
                if (company == null)
                {
                    return NotFound();
                }
                return View(company);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company company) {
            if (!ModelState.IsValid) return View(company);
            var message = "";
            if (company.Id == 0)
            {
                _repo.Add(company);
                message = "Company has been added successfully";
            }
            else
            {
                _repo.Update(company);
                message = "Company has been updated successfully";
            }
            _repo.Save();
            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _repo.GetAll();
            return Json(new { data = companies });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _repo.Get(id.GetValueOrDefault());
            if (companyToBeDeleted == null) return Json(new { success = false, message = "Error while deleting" });
            _repo.Remove(companyToBeDeleted);
            _repo.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
