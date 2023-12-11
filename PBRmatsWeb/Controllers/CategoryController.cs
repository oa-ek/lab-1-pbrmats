using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;
using PBRmats.Application.DTOs;

namespace PBRmatsWeb.Controllers
{
    [Authorize(Roles = "RootAdmin, Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category, int> _categoryRepository;

        public CategoryController(IRepository<Category, int> categoryRepository) => 
            _categoryRepository = categoryRepository;

        private Category GetCategory(int id) =>
            _categoryRepository.Get(id);

        private CategoryDTO SetDTO(int id)
        {
            var categoryEntity = GetCategory(id);

            return new CategoryDTO() { Id = categoryEntity.Id, Title = categoryEntity.Title };
        }

        public IActionResult Index() =>
            View(_categoryRepository.GetAll());

        [HttpGet]
        public IActionResult Create() =>
            View("Create");

        public IActionResult Edit(int id) =>
            View(SetDTO(id));

        public IActionResult Delete(int id) =>
            View(SetDTO(id));

        [HttpPost]
        public IActionResult Create(CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                var newCategory = new Category() { Title = category.Title };
                _categoryRepository.Create(newCategory);

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Edit(CategoryDTO category)
        {
            var categoryToEdit = GetCategory(category.Id);
            categoryToEdit.Title = category.Title;
            _categoryRepository.Update(categoryToEdit);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(CategoryDTO category)
        {
            var categoryToDelete = GetCategory(category.Id);
            _categoryRepository.Delete(categoryToDelete);

            return RedirectToAction("Index");
        }
    }
}
