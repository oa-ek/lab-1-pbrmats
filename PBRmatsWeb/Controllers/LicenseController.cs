using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;

namespace PBRmatsWeb.Controllers
{
    [Authorize(Roles = "RootAdmin, Admin")]
    public class LicenseController : Controller
    {
        private readonly IRepository<License, int> _licenseRepository;

        public LicenseController(IRepository<License, int> licenseRepository)
        {
            _licenseRepository = licenseRepository;
        }

        public IActionResult Index()
        {
            return View(_licenseRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(License license)
        {
            if (ModelState.IsValid)
            {
                _licenseRepository.Create(license);

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            return View(_licenseRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Delete(License license)
        {
            _licenseRepository.Delete(license);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_licenseRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(License license)
        {
            _licenseRepository.Update(license);

            return RedirectToAction("Index");
        }
    }
}
