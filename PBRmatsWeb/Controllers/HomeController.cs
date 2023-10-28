using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;
using PBRmats.Repositories.Repos;
using PBRmatsWeb.Models;
using System.Diagnostics;

namespace PBRmatsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Material, int> _materialRepository;
        private readonly IListService<Category> _categoryService;
        private readonly IListService<License> _licenseService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IRepository<Material, int> materialRepository,
                                    IListService<Category> categoryService, 
                                    IListService<License> licenseService,
                                    ILogger<HomeController> logger)
        {
            _materialRepository = materialRepository;
            _categoryService = categoryService;
            _licenseService = licenseService;
            _logger = logger;
        }
        private IEnumerable<Material> GetFilteredMaterials(int? categoryId)
        {
            var materialsQuery = _materialRepository.GetAll();

            if (categoryId.HasValue)
            {
                materialsQuery = materialsQuery.Where(material => material.CategoryId == categoryId);
            }

            var categories = _categoryService.GetList();
            var licenses = _licenseService.GetList();

            return materialsQuery
                .Select(material =>
                {
                    material.Category = categories.FirstOrDefault(c => c.Id == material.CategoryId);
                    material.License = licenses.FirstOrDefault(l => l.Id == material.LicenseId);
                    return material;
                })
                .ToList();
        }


        public IActionResult Index(int? categoryId)
        {
            GetData();
            var materials = GetFilteredMaterials(categoryId);
            return View(materials);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void GetData()
        {
            ViewData["Categories"] = _categoryService.GetList();
            ViewData["Licenses"] = _licenseService.GetList();
        }
    }
}