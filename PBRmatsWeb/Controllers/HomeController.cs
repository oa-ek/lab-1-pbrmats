using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;
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

        private IEnumerable<Material> GetFilteredMaterials(string searchTerm = "", string sortBy = "", 
                                                            int? categoryId = null, int? licenseSort = null)
        {
            var materialsQuery = _materialRepository.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                materialsQuery = materialsQuery.Where(m => m.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId.HasValue)
                materialsQuery = materialsQuery.Where(material => material.CategoryId == categoryId);

            if (licenseSort.HasValue)
                materialsQuery = materialsQuery.Where(material => material.LicenseId == licenseSort);

            var categories = _categoryService.GetList();
            var licenses = _licenseService.GetList();

            materialsQuery = materialsQuery
            .Select(material =>
            {
                material.Category = categories.FirstOrDefault(c => c.Id == material.CategoryId);
                material.License = licenses.FirstOrDefault(l => l.Id == material.LicenseId);
                return material;
            })
            .ToList();

            switch (sortBy)
            {
                case "dateAsc":
                    materialsQuery = materialsQuery.OrderBy(material => material.ReleaseDate);
                    break;
                case "titleAsc":
                    materialsQuery = materialsQuery.OrderBy(material => material.Title);
                    break;
                case "titleDesc":
                    materialsQuery = materialsQuery.OrderByDescending(material => material.Title);
                    break;
                default: //"dateDesc"
                    return materialsQuery = materialsQuery.OrderByDescending(material => material.ReleaseDate);
            }


            return materialsQuery;
        }

        public IActionResult Index(string searchTerm = "", string sortBy = "", 
                                    int? categoryId = null, int? licenseSort = null)
        {
            GetData();
            var materials = GetFilteredMaterials(searchTerm, sortBy, categoryId, licenseSort);

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