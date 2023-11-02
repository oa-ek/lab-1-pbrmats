using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;
using Newtonsoft.Json;
using PBRmats.Core.Context;
using Microsoft.EntityFrameworkCore;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using PBRmats.Core.Migrations;
using System.IO;

namespace PBRmatsWeb.Controllers
{
    file class TagDTO
    {
        public string Value { get; set; }
    }

    [Authorize(Roles = "Admin")]
    public class MaterialController : Controller
    {
        private const string DefaultImage = "/uploads/defaultMaterial.webp";
        private const string UploadsFolder = "uploads";
        private const string MaterialFolder = "Material";
        private const string DefaultHostUrl = "https://localhost:7072/";

        private readonly IRepository<Material, int> _materialRepository;
        private readonly IRepository<Tag, int> _tagRepository;
        private readonly IListService<Category> _categoryService;
        private readonly IListService<License> _licenseService;
        private readonly IWebHostEnvironment _environment;

        private PBRmatsContext _context;

        public MaterialController(PBRmatsContext context, IRepository<Material, int> materialRepository,
                                    IRepository<Tag, int> tagRepository,
                                    IListService<Category> categoryService,
                                    IListService<License> licenseService,
                                    IWebHostEnvironment environment)
        {
            _materialRepository = materialRepository;
            _tagRepository = tagRepository;
            _categoryService = categoryService;
            _licenseService = licenseService;
            _environment = environment;
            _context = context;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material, IFormFile MaterialImage, string MaterialTags)
        {
            material.ImageUrl = await SaveMaterialImageAsync(MaterialImage);

            ParseAndAddTags(material, MaterialTags);

            _materialRepository.Create(material);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            PopulateDropdowns();

            var material = _materialRepository.Get(id);

            if (material == null)
                return NotFound();

            return View(material);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Material material, IFormFile MaterialImage, string MaterialTags)
        {
            material.ImageUrl = await SaveMaterialImageAsync(MaterialImage, material.ImageUrl);

            //ParseAndAddTags(material, MaterialTags);

            _materialRepository.Update(material);

            return RedirectToAction(nameof(Index));
        }

        private void ParseAndAddTags(Material material, string MaterialTags)
        {
            if (string.IsNullOrWhiteSpace(MaterialTags))
                return;

            var tags = JsonConvert.DeserializeObject<List<TagDTO>>(MaterialTags)
                                    .Select(t => t.Value.Trim())
                                    .Distinct(StringComparer.OrdinalIgnoreCase)
                                    .ToList();

            foreach (var title in tags)
            {
                var existingTag = _context.Tags.FirstOrDefault(t => t.Title.ToLower() == title.ToLower());
                if (existingTag == null)
                {
                    existingTag = new Tag { Title = title };
                    _context.Tags.Add(existingTag);
                    _context.SaveChanges(); // Ensure that the tag gets an ID if it's new
                }

                if (material.MaterialTags == null)
                    material.MaterialTags = new List<MaterialTag>();

                // Check if the association already exists
                if (!material.MaterialTags.Any(mt => mt.TagsId == existingTag.Id))
                {
                    material.MaterialTags.Add(new MaterialTag { Tag = existingTag });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetToDefaultImage(int materialId)
        {
            var material = _materialRepository.Get(materialId);

            if (material != null)
            {
                material.ImageUrl = DefaultImage;
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
            ViewData["Categories"] = new SelectList(_categoryService.GetList(), nameof(Category.Id), nameof(Category.Title));
            ViewData["Licenses"] = new SelectList(_licenseService.GetList(), nameof(License.Id), nameof(License.Title));
        }
    }
}
