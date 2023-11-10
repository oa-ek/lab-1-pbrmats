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
        private readonly IRepository<AppUser, string> _appUserRepository;
        private readonly IRepository<MaterialsCollection, int> _collectionRepository;
        private readonly IRepository<Material, int> _materialRepository;
        public CollectionController(IRepository<AppUser, string> appUserRepository,
                                    IRepository<MaterialsCollection, int> collectionRepository,
                                    IRepository<Material, int> materialRepository)
        {
            _collectionRepository = collectionRepository;
            _appUserRepository = appUserRepository;
            _materialRepository = materialRepository;
        }

        public async Task<IActionResult> Index([FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.GetUserAsync(User);

            var collections = _collectionRepository.GetAll().Where(c => c.AppUserId == user.Id);

            return View(collections);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string title, string cardColor, [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.GetUserAsync(User);
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
        public async Task<IActionResult> AddMaterialToCollection(int materialId, int collectionId, [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User must be logged in." });
            }

            var collection = _collectionRepository.Get(collectionId);
            if (collection == null || collection.AppUserId != user.Id)
            {
                return Json(new { success = false, message = "Collection not found or user does not have permission." });
            }

            var material = _materialRepository.Get(materialId);
            if (material == null)
            {
                return Json(new { success = false, message = "Material not found." });
            }

            if (collection.MaterialMaterialsCollection == null)
            {
                collection.MaterialMaterialsCollection = new List<MaterialMaterialsCollection>();
            }

            if (!collection.MaterialMaterialsCollection.Any(mmc => mmc.MaterialId == material.Id && mmc.MaterialsCollectionId == collection.Id))
            {
                collection.MaterialMaterialsCollection.Add(new MaterialMaterialsCollection { Material = material, MaterialsCollection = collection });
            }
            else
            {
                return Json(new { success = false, message = "Material is already in the collection." });
            }

            _collectionRepository.Update(collection);

            return Json(new { success = true, message = "Material added to collection successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveMaterialFromCollection(int materialId, int collectionId, [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User must be logged in." });
            }

            var collection = _collectionRepository.Get(collectionId);
            if (collection == null || collection.AppUserId != user.Id)
            {
                return Json(new { success = false, message = "Collection not found or user does not have permission." });
            }

            var materialInCollection = collection.MaterialMaterialsCollection.FirstOrDefault(mmc => mmc.MaterialId == materialId);
            if (materialInCollection != null)
            {
                collection.MaterialMaterialsCollection.Remove(materialInCollection);
                _collectionRepository.Update(collection);
                return Json(new { success = true, message = "Material removed from collection successfully." });
            }

            return Json(new { success = false, message = "Material is not in the collection." });
        }

        public IActionResult Delete(int id)
        {
            return View(_collectionRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Delete(MaterialsCollection materialsCollection)
        {
            _collectionRepository.Delete(materialsCollection);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> View(int id, [FromServices] UserManager<IdentityUser> userManager)
        {
            IEnumerable<MaterialsCollection> collections;

            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                collections = _collectionRepository.GetAll().Where(c => c.AppUserId == user.Id);
            }
            else
            {
                collections = new List<MaterialsCollection>();
            }

            return View((_collectionRepository.Get(id), collections));
        }
    }
}
