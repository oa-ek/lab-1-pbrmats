using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBRmats.Application.DTOs;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;
using System.ComponentModel;

namespace PBRmatsWeb.Controllers
{
    [Authorize(Roles = "RootAdmin, Admin")]
    public class TagController : Controller
    {
        private readonly IRepository<Tag, int> _tagRepository;

        public TagController(IRepository<Tag, int> tagRepository) => 
            _tagRepository = tagRepository;

        private Tag GetTag(int id) =>
            _tagRepository.Get(id);

        private TagDTO SetDTO(int id)
        {
            var tagEntity = GetTag(id);

            return new TagDTO() { Id = tagEntity.Id, Title = tagEntity.Title };
        }

        public IActionResult Index() =>
            View(_tagRepository.GetAll());

        [HttpGet]
        public IActionResult Create() =>
            View("Create");

        public IActionResult Edit(int id) =>
            View(SetDTO(id));

        public IActionResult Delete(int id) =>
            View(SetDTO(id));

        [HttpPost]
        public IActionResult Create(TagDTO tag)
        {
            if (ModelState.IsValid)
            {
                var newTag = new Tag() { Title = tag.Title, MaterialTags = null};
                _tagRepository.Create(newTag);

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Edit(TagDTO tag)
        {
            var tagToEdit = GetTag(tag.Id);
            tagToEdit.Title = tag.Title;
            _tagRepository.Update(tagToEdit);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(TagDTO tag)
        {
            var tagToDelete = GetTag(tag.Id);
            _tagRepository.Delete(tagToDelete);

            return RedirectToAction("Index");
        }
    }
}
