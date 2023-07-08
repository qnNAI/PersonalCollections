using System.Security.Claims;
using Application.Common.Contracts.Services;
using Application.Models.Collection;
using Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalCollections.Filters;
using PersonalCollections.Models;

namespace PersonalCollections.Controllers
{

    public class CollectionController : Controller
    {
        private readonly ICollectionService _collectionService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CollectionController(ICollectionService collectionService, UserManager<ApplicationUser> userManager)
        {
            _collectionService = collectionService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [PersonalInfoFilter]
        public IActionResult AddCollection([FromQuery] string userId)
        {
            return View(new AddCollectionRequest { UserId = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddCollection(AddCollectionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            if (!User.IsInRole("Admin") && request.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            await _collectionService.AddAsync(request);

            return RedirectToAction(nameof(CollectionsManagement), new { request.UserId });
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
            var collections = await _collectionService.GetCollectionsAsync(userId);
            var author = (await _userManager.FindByIdAsync(userId)).Adapt<AuthorDto>();

            var response = new CollectionsManagementResponse
            {
                Author = author,
                Collections = collections
            };
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Collection([FromQuery] string collectionId) {
            var collection = await _collectionService.GetByIdAsync(collectionId);
            if (collection is null) {
                return View("Error", new ErrorViewModel { Message = $"Collection with id={collectionId} not found!" });
            }

            return View(collection);
        }
    }
}
