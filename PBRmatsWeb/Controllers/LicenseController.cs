using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBRmats.Application.DTOs;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;

namespace PBRmatsWeb.Controllers
{
    [Authorize(Roles = "RootAdmin, Admin")]
    public class LicenseController : Controller
    {
        private readonly IRepository<License, int> _licenseRepository;

        public LicenseController(IRepository<License, int> licenseRepository) => 
            _licenseRepository = licenseRepository;

        private License GetLicense(int id) =>
            _licenseRepository.Get(id);

        private LicenseDTO SetDTO(int id)
        {
            var categoryEntity = GetLicense(id);

            return new LicenseDTO() { Id = categoryEntity.Id, Title = categoryEntity.Title };
        }

        public IActionResult Index() =>
            View(_licenseRepository.GetAll());

        [HttpGet]
        public IActionResult Create() =>
            View("Create");

        public IActionResult Edit(int id)
        {
            return View(SetDTO(id));
        }

        public IActionResult Delete(int id)
        {
            return View(SetDTO(id));
        }

        [HttpPost]
        public IActionResult Create(LicenseDTO license)
        {
            if (ModelState.IsValid)
            {
                var newLicense = new License() { Title = license.Title };
                _licenseRepository.Create(newLicense);

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Edit(LicenseDTO license)
        {
            var licenseToEdit = GetLicense(license.Id);
            licenseToEdit.Title = license.Title;
            _licenseRepository.Update(licenseToEdit);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(LicenseDTO license)
        {
            var licenseToDelete = GetLicense(license.Id);
            _licenseRepository.Delete(licenseToDelete);

            return RedirectToAction("Index");
        }
    }
}
