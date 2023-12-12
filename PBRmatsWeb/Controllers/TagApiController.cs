using Microsoft.AspNetCore.Mvc;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;

namespace PBRmats.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagApiController : ControllerBase
    {
        private readonly IRepository<Tag, int> _tagRepository;

        public TagApiController(IRepository<Tag, int> tagRepository) =>
            _tagRepository = tagRepository;

        [HttpGet]
        public IEnumerable<Tag> GetAll() =>
            _tagRepository.GetAll();

        [HttpGet("{id}")]
        public Tag Get(int id) =>
            _tagRepository.Get(id);

        [HttpPost]
        public ActionResult<Tag> Create(Tag tag)
        {
            tag.MaterialTags = null;
            _tagRepository.Create(tag);

            return CreatedAtAction(nameof(Get), new { id = tag.Id }, tag);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Tag tag)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tagToEdit = _tagRepository.Get(id);
            if (tagToEdit == null)
            {
                return NotFound();
            }

            tagToEdit.Title = tag.Title;

            _tagRepository.Update(tagToEdit);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tagToDelete = _tagRepository.Get(id);
            if (tagToDelete == null)
            {
                return NotFound();
            }

            _tagRepository.Delete(tagToDelete);

            return NoContent();
        }
    }
}