using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;
using PBRmats.Repositories.Repos;
using System.Linq;

namespace PBRmatsWeb.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IRepository<Material, int> _materialRepository;
        private readonly IListService<Category> _categoryService;
        private readonly IListService<License> _licenseService;

        public MaterialController(IRepository<Material, int> materialRepository, 
                                    IListService<Category> categoryService, IListService<License> licenseService)
        {
            _materialRepository = materialRepository;
            _categoryService = categoryService;
            _licenseService = licenseService;
        }

        public IActionResult Index()
        {
            var materials = _materialRepository.GetAll()
                .Select(material =>
                {
                    material.Category = _categoryService
                                            .GetList()
                                            .FirstOrDefault(c => c.Id == material.CategoryId);

                    material.License = _licenseService
                                            .GetList()
                                            .FirstOrDefault(l => l.Id == material.LicenseId);

                    return material;
                })
                .ToList();

            return View(materials);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateDropdowns();

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Material material)
        {
            _materialRepository.Create(material);

            PopulateDropdowns();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            return View(_materialRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Delete(Material material)
        {
            _materialRepository.Delete(material);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            PopulateDropdowns();

            return View(_materialRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(Material material)
        {
            _materialRepository.Update(material);
            PopulateDropdowns();

            return RedirectToAction("Index");
        }

        private void PopulateDropdowns()
        {
            ViewData["Categories"] = new SelectList(_categoryService.GetList(), nameof(Category.Id), nameof(Category.Title));
            ViewData["Licenses"] = new SelectList(_licenseService.GetList(), nameof(License.Id), nameof(License.Title));
        }
    }
}
