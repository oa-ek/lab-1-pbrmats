using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;

namespace PBRmatsWeb.Controllers
{
    [Authorize(Roles = "RootAdmin, Admin")]
    public class TagController : Controller
    {
        private readonly IRepository<Tag, int> _tagRepository;

        public TagController(IRepository<Tag, int> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            return View(_tagRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
                _tagRepository.Create(tag);

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            return View(_tagRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Delete(Tag tag)
        {
            _tagRepository.Delete(tag);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_tagRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            _tagRepository.Update(tag);

            return RedirectToAction("Index");
        }
    }
}
