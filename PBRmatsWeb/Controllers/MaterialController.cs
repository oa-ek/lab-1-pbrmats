using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;
using Newtonsoft.Json;
using PBRmats.Application.DTOs;
using System.ComponentModel;

namespace PBRmatsWeb.Controllers
{
    file class TagDTO
    {
        public string Value { get; set; }
    }

    [Authorize(Roles = "RootAdmin, Admin")]
    public class MaterialController : Controller
    {
        private const string DefaultImage = "/uploads/defaultMaterial.webp";
        private const string UploadsFolder = "uploads";
        private const string MaterialFolder = "Material";
        private const string DefaultHostUrl = "https://localhost:7072/";

        private readonly IRepository<Material, int> _materialRepository;
        private readonly IRepository<Tag, int> _tagRepository;
        private readonly IRepository<Category, int> _categoryRepository;
        private readonly IRepository<PBRmats.Core.Entities.License, int> _licenseRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<MaterialController> _logger;

        public MaterialController(IRepository<Material, int> materialRepository,
                                    IRepository<Tag, int> tagRepository,
                                    IRepository<Category, int> categoryRepository,
                                    IRepository<PBRmats.Core.Entities.License, int> licenseRepository,
                                    IWebHostEnvironment environment,
                                    ILogger<MaterialController> logger)
        {
            _materialRepository = materialRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _licenseRepository = licenseRepository;
            _environment = environment;
            _logger = logger;
        }

        private Material GetMaterial(int id) =>
            _materialRepository.Get(id);

        private MaterialDTO SetDTO(int id)
        {
            var tagEntity = GetMaterial(id);

            return new MaterialDTO() { Id = tagEntity.Id, Title = tagEntity.Title };
        }

        public IActionResult Index()
        {
            var materials = _materialRepository.GetAll();

            var updatedMaterials = materials.Select(material =>
            {
                material.ImageUrl = GetImageByMaterial(material.Id.ToString());

                return material;
            });

            return View(updatedMaterials);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateDropdowns();

            return View();
        }

        public IActionResult Edit(int id)
        {
            PopulateDropdowns();

            return View(GetMaterial(id));
        }

        public IActionResult Delete(int id) =>
            View(SetDTO(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialDTO material, IFormFile? MaterialImage, IFormFile? MaterialZipFile, string MaterialTags)
        {
            var newMaterial = new Material() 
            { 
                Title = material.Title,
                AvgColor = material.AvgColor,
                AvgSpecularColor = material.AvgSpecularColor,
                AvgMetallic = material.AvgMetallic,
                AvgIOR = material.AvgIOR,
                ReleaseDate = material.ReleaseDate,
                LicenseId = material.LicenseId,
                CategoryId = material.CategoryId,
                ImageUrl = material.ImageUrl,
                ZipFileUrl = material.ZipFileUrl,
                License = _licenseRepository.Get(material.LicenseId),
                Category = _categoryRepository.Get(material.CategoryId),
                MaterialTags = null,
                MaterialMaterialsCollection = null
            };

            if (MaterialZipFile != null && MaterialZipFile.Length > 0)
                material.ZipFileUrl = await SaveMaterialZipFileAsync(MaterialZipFile);

            material.ImageUrl = await SaveMaterialImageAsync(MaterialImage);

            ParseAndAddTags(newMaterial, MaterialTags);

            _materialRepository.Create(newMaterial);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Material material, IFormFile? MaterialImage, IFormFile? MaterialZipFile, string MaterialTags)
        {
            if (MaterialZipFile != null)
            {
                DeleteMaterialZipFile(material.ZipFileUrl);

                material.ZipFileUrl = await SaveMaterialZipFileAsync(MaterialZipFile);
            }
            else
            {
                material.ZipFileUrl = string.Empty;
            }

            material.ImageUrl = await SaveMaterialImageAsync(MaterialImage, material.ImageUrl);
            ParseAndAddTags(material, MaterialTags);

            _materialRepository.Update(material);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(MaterialDTO material)
        {
            var materialToDelete = GetMaterial(material.Id);
            DeleteMaterialZipFile(material.ZipFileUrl);
            _materialRepository.Delete(materialToDelete);

            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveMaterialZipFileAsync(IFormFile? materialZipFile)
        {
            if (materialZipFile == null)
                return Path.Combine("/uploads/Material/zips/", materialZipFile.FileName); // Return the relative path
            
            var uploadsFolderPath = Path.Combine(_environment.WebRootPath, "uploads", "Material", "zips");
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var filePath = Path.Combine(uploadsFolderPath, materialZipFile.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await materialZipFile.CopyToAsync(fileStream);
            }

            return Path.Combine("/uploads/Material/zips/", materialZipFile.FileName); // Return the relative path
        }

        private void ParseAndAddTags(Material material, string MaterialTags)
        {
            if (string.IsNullOrWhiteSpace(MaterialTags))
                return;

            var tags = JsonConvert.DeserializeObject<List<TagDTO>>(MaterialTags)
                                    .Select(t => t.Value.Trim())
                                    .Distinct(StringComparer.OrdinalIgnoreCase)
                                    .ToList();

            var existingTags = _tagRepository.GetAll();

            foreach (var title in tags)
            {
                var existingTag = existingTags.FirstOrDefault(t => string.Equals(t.Title, title, StringComparison.CurrentCultureIgnoreCase));
                if (existingTag == null)
                {
                    existingTag = new Tag { Title = title };
                    _tagRepository.Create(existingTag);
                }

                material.MaterialTags ??= new List<MaterialTag>();

                // Check if the MaterialTag already exists
                if (!material.MaterialTags.Any(mt => mt.TagsId == existingTag.Id && mt.MaterialId == material.Id))
                {
                    material.MaterialTags.Add(new MaterialTag { Tag = existingTag, Material = material });
                }
            }
        }

        private void DeleteMaterialZipFile(string zipFileUrl)
        {
            if (string.IsNullOrEmpty(zipFileUrl))
                return;

            var filePath = Path.Combine(_environment.WebRootPath, zipFileUrl.TrimStart('/'));
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file {FilePath}", filePath);
            }
        }

        private async Task<string> SaveMaterialImageAsync(IFormFile? materialImage, string currentImageUrl = null)
        {
            // If an image is uploaded
            if (materialImage != null)
            {
                var path = Path.Combine(_environment.WebRootPath, "uploads", "Material");
                Directory.CreateDirectory(path);
                var filename = materialImage.FileName;

                // If there's a current image that's not the default, delete it
                if (!string.IsNullOrEmpty(currentImageUrl) && currentImageUrl != DefaultImage)
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
            return DefaultImage;
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
            string imagePath = Path.Combine(GetFilePath(materialCode), "image.png");

            if (!System.IO.File.Exists(imagePath))
            {
                imageUrl = Path.Combine(DefaultHostUrl, "uploads", "common", "noimage.png");
            }
            else
            {
                imageUrl = Path.Combine(DefaultHostUrl, "uploads", "Material", materialCode, "image.png");
            }

            return imageUrl;
        }

        private void PopulateDropdowns()
        {
            ViewData["Categories"] = new SelectList(_categoryRepository.GetAll(), nameof(Category.Id), nameof(Category.Title));
            ViewData["Licenses"] = new SelectList(_licenseRepository.GetAll(), nameof(PBRmats.Core.Entities.License.Id), nameof(PBRmats.Core.Entities.License.Title));
        }
    }
}
