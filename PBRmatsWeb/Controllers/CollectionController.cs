using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;

namespace PBRmatsWeb.Controllers
{
    [Authorize(Roles = "RootAdmin, Admin, User")]
    public class CollectionController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AppUser, string> _appUserRepository;
        private readonly IRepository<MaterialsCollection, int> _collectionRepository;
        public CollectionController(UserManager<IdentityUser> userManager,
                                    IRepository<AppUser, string> appUserRepository,
                                    IRepository<MaterialsCollection, int> collectionRepository)
        {
            _userManager = userManager;
            _collectionRepository = collectionRepository;
            _appUserRepository = appUserRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var collections = _collectionRepository.GetAll().Where(c => c.AppUserId == user.Id);

            return View(collections);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string title, string cardColor)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var usersIDs = _appUserRepository.GetAll();
            if(!usersIDs.Any(u => u.Id == user.Id))
                _appUserRepository.Create(new AppUser { Id = user.Id });

            var newCollection = new MaterialsCollection
            {
                Title = title,
                CardColor = cardColor,
                AppUserId = user.Id
            };

            _collectionRepository.Create(newCollection);

            return Json(new { success = true, message = "Its work :)" });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var collection = _collectionRepository.Get(id);
            if (collection == null)
            {
                return Json(new { success = false, message = "Collection not found." });
            }

            _collectionRepository.Delete(collection);

            return Json(new { success = true, message = "Collection deleted successfully." });
        }

        public IActionResult View(int id)
        {
            return View(_collectionRepository.Get(id));
        }
    }
}
