using System.Security.Claims;
using Application.Common.Contracts.Services;
using Application.Models.Collection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalCollections.Filters;

namespace PersonalCollections.Controllers
{

    public class CollectionController : Controller
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        [HttpGet]
        public IActionResult AddCollection()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PersonalInfoFilter]
        public async Task<IActionResult> AddCollection(AddCollectionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var userId = TempData.Peek("UserId")?.ToString();
            request.UserId = userId;

            await _collectionService.AddAsync(request);

            return RedirectToAction(nameof(CollectionsManagement), new { userId });
        }

        [HttpPost]
        [Authorize]
        [PersonalInfoFilter]
        public async Task<IActionResult> RemoveCollection(string collectionId)
        {
            var result = await _collectionService.RemoveAsync(collectionId);
            return result.Succeeded ? Ok() : BadRequest();
        }

        [HttpGet]
        [Authorize]
        [PersonalInfoFilter]
        public async Task<IActionResult> CollectionsManagement(string userId)
        {
            TempData["UserId"] = userId;
            var collections = await _collectionService.GetCollectionsAsync(userId);
            return View(collections);
        }
    }
}
