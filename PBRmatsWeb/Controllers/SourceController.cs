using Microsoft.AspNetCore.Mvc;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;

namespace PBRmatsWeb.Controllers
{
    public class SourceController : Controller
    {
        private readonly IRepository<Source, int> _sourceRepository;

        public SourceController(IRepository<Source, int> sourceRepository)
        {
            _sourceRepository = sourceRepository;
        }

        public IActionResult Index()
        {
            return View(_sourceRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Source source)
        {
            if (ModelState.IsValid)
            {
                _sourceRepository.Create(source);

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            return View(_sourceRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Delete(Source source)
        {
            _sourceRepository.Delete(source);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_sourceRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(Source source)
        {
            _sourceRepository.Update(source);

            return RedirectToAction("Index");
        }
    }
}
