using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;

namespace PBRmatsWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category, int> _categoryRepository;

        public CategoryController(IRepository<Category, int> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View(_categoryRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Create(category);

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            return View(_categoryRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            _categoryRepository.Delete(category);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_categoryRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _categoryRepository.Update(category);

            return RedirectToAction("Index");
        }
    }
}
