﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace PBRmatsWeb.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IRepository<Material, int> _materialRepository;
        private readonly IListService<Category> _categoryService;
        private readonly IListService<License> _licenseService;
        private readonly IWebHostEnvironment _environment;

        public MaterialController(IRepository<Material, int> materialRepository, 
                                    IListService<Category> categoryService,
                                    IListService<License> licenseService,
                                    IWebHostEnvironment environment)
        {
            _materialRepository = materialRepository;
            _categoryService = categoryService;
            _licenseService = licenseService;
            _environment = environment;
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

                    material.ImageUrl = GetImageByMaterial(material.Id.ToString());

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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Material material)
        //{
        //    _materialRepository.Create(material);

        //    PopulateDropdowns();

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material, IFormFile MaterialImage)
        {
            material.ImageUrl = await SaveMaterialImageAsync(MaterialImage);
            _materialRepository.Create(material);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            PopulateDropdowns();

            return View(_materialRepository.Get(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Material material, IFormFile MaterialImage)
        {
            material.ImageUrl = await SaveMaterialImageAsync(MaterialImage, material.ImageUrl);
            _materialRepository.Update(material);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetToDefaultImage(int materialId)
        {
            var material = _materialRepository.Get(materialId);
            if (material != null)
            {
                material.ImageUrl = "/uploads/defaultMaterial.webp";
                _materialRepository.Update(material);
            }

            return View(_materialRepository.Get(materialId));
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

        private async Task<string> SaveMaterialImageAsync(IFormFile materialImage, string currentImageUrl = null)
        {
            // If an image is uploaded
            if (materialImage != null)
            {
                var path = Path.Combine(_environment.WebRootPath, "uploads", "Material");
                Directory.CreateDirectory(path);
                var filename = materialImage.FileName;

                // If there's a current image that's not the default, delete it
                if (!string.IsNullOrEmpty(currentImageUrl) && currentImageUrl != "/uploads/defaultMaterial.webp")
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, currentImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                // Save the new image
                using (var stream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                {
                    await materialImage.CopyToAsync(stream);
                }
                return Path.Combine("/uploads/Material/", filename);
            }

            // If no new image is uploaded but there's an existing image
            if (!string.IsNullOrEmpty(currentImageUrl))
                return currentImageUrl;

            // If there's no new or existing image, use the default
            return "/uploads/defaultMaterial.webp";
        }

        [HttpGet("RemoveImage/{code}")]
        public ActionResult RemoveImage(string code)
        {
            string Filepath = GetFilePath(code);
            string Imagepath = Filepath + "\\image.png";
            try
            {
                if (System.IO.File.Exists(Imagepath))
                    System.IO.File.Delete(Imagepath);

                return View(code);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        [NonAction]
        private string GetFilePath(string ProductCode)
        {
            return _environment.WebRootPath + "\\Uploads\\Material\\" + ProductCode;
        }
        [NonAction]
        private string GetImageByMaterial(string materialCode)
        {
            string imageUrl;
            string hostUrl = "https://localhost:7072/";
            string imagePath = Path.Combine(GetFilePath(materialCode), "image.png");

            if (!System.IO.File.Exists(imagePath))
            {
                imageUrl = Path.Combine(hostUrl, "uploads", "common", "noimage.png");
            }
            else
            {
                imageUrl = Path.Combine(hostUrl, "uploads", "Material", materialCode, "image.png");
            }
            return imageUrl;

        }

        private void PopulateDropdowns()
        {
            ViewData["Categories"] = new SelectList(_categoryService.GetList(), nameof(Category.Id), nameof(Category.Title));
            ViewData["Licenses"] = new SelectList(_licenseService.GetList(), nameof(License.Id), nameof(License.Title));
        }
    }
}
