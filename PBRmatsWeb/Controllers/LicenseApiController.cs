using Microsoft.AspNetCore.Mvc;
using PBRmats.Repositories.Interfaces;

namespace PBRmats.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseApiController : ControllerBase
    {
        private readonly IRepository<Core.Entities.License, int> _licenseRepository;

        public LicenseApiController(IRepository<Core.Entities.License, int> licenseRepository) => 
            _licenseRepository = licenseRepository;

        [HttpGet]
        public IEnumerable<Core.Entities.License> GetAll() =>
            _licenseRepository.GetAll();

        [HttpGet("{id}")]
        public Core.Entities.License Get(int id) =>
            _licenseRepository.Get(id);

        [HttpPost]
        public ActionResult<Core.Entities.License> Create(Core.Entities.License license)
        {
            _licenseRepository.Create(license);

            return CreatedAtAction(nameof(Get), new { id = license.Id }, license);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Core.Entities.License license)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var licenseToEdit = _licenseRepository.Get(id);
            if (licenseToEdit == null)
            {
                return NotFound();
            }

            licenseToEdit.Title = license.Title;

            _licenseRepository.Update(licenseToEdit);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var licenseToDelete = _licenseRepository.Get(id);
            if (licenseToDelete == null)
            {
                return NotFound();
            }

            _licenseRepository.Delete(licenseToDelete);

            return NoContent();
        }
    }
}
