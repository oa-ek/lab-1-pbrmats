using Microsoft.AspNetCore.Mvc;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;

namespace PBRmats.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly IRepository<Category, int> _categoryRepository;

        public CategoryApiController(IRepository<Category, int> categoryRepository) =>
            _categoryRepository = categoryRepository;

        [HttpGet]
        public IEnumerable<Category> GetAll() =>
            _categoryRepository.GetAll();

        [HttpGet("{id}")]
        public Category Get(int id) =>
            _categoryRepository.Get(id);

        [HttpPost]
        public ActionResult<Category> Create(Category category)
        {
            _categoryRepository.Create(category);

            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryToEdit = _categoryRepository.Get(id);
            if (categoryToEdit == null)
            {
                return NotFound();
            }

            categoryToEdit.Title = category.Title;

            _categoryRepository.Update(categoryToEdit);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var categoryToDelete = _categoryRepository.Get(id);
            if (categoryToDelete == null)
            {
                return NotFound();
            }

            _categoryRepository.Delete(categoryToDelete);

            return NoContent();
        }
    }
}
