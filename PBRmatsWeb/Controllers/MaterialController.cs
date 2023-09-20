using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;
using PBRmats.Repositories.Repos;

namespace PBRmatsWeb.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IRepository<Material, int> _materialRepository;

        public MaterialController(IRepository<Material, int> materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public IActionResult Index()
        {
            return View(_materialRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }
        
        [HttpPost]
        public IActionResult Create(Material material)
        {
            _materialRepository.Create(material);
            return RedirectToAction("Index");
            if (ModelState.IsValid)
            {
            }

            return View();
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
            return View(_materialRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(Material material)
        {
            _materialRepository.Update(material);

            return RedirectToAction("Index");
        }
    }
}
