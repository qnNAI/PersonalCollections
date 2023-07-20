using System.Security.Claims;
using Application.Common.Contracts.Services;
using Application.Models.Collection;
using Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonalCollections.Filters;
using PersonalCollections.Models;

namespace PersonalCollections.Controllers
{

    public class CollectionController : Controller
    {
        private readonly ICollectionService _collectionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<CollectionController> _localizer;

        public CollectionController(ICollectionService collectionService, UserManager<ApplicationUser> userManager, IStringLocalizer<CollectionController> localizer)
        {
            _collectionService = collectionService;
            _userManager = userManager;
            _localizer = localizer;
        }

        [HttpGet]
        [Authorize]
        [AuthorFilter]
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

        [HttpGet]
        [Authorize]
        [AuthorFilter]
        public async Task<IActionResult> EditCollection([FromQuery] string collectionId)
        {
            var collection = await _collectionService.GetByIdAsync(collectionId);
            if (collection is null)
            {
                return View("Error", new ErrorViewModel { Message = _localizer["NotFound"].Value });
            }

            return View(collection.Adapt<EditCollectionRequest>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditCollection(EditCollectionRequest request)
        {
            if(!ModelState.IsValid)
            {
                var collection = await _collectionService.GetByIdAsync(request.Id);
                if (collection is null)
                {
                    return View("Error", new ErrorViewModel { Message = _localizer["NotFound"].Value });
                }
                return await _EditCollectionFailedResponseAsync(collection, request);
            }

            if(!User.IsInRole("Admin") && request.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            var result = await _collectionService.UpdateAsync(request);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(CollectionsManagement), new { userId = request.UserId });
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }

                var collection = await _collectionService.GetByIdAsync(request.Id);
                return await _EditCollectionFailedResponseAsync(collection, request);
            }
        }

        [HttpPost]
        [Authorize]
        [AuthorFilter]
        public async Task<IActionResult> RemoveCollection(string collectionId)
        {
            var result = await _collectionService.RemoveAsync(collectionId);
            return result.Succeeded ? Ok() : BadRequest();
        }

        [HttpGet]
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
                return View("Error", new ErrorViewModel { Message = _localizer["NotFound"].Value });
            }

            return View(collection);
        }

        public async Task<IActionResult> SearchCollections(string term, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var collections = await _collectionService.GetCollectionsAsync(term, page, pageSize, cancellationToken);

            return PartialView("_SearchCollectionsResultPartial", collections);
        }

        private async Task<IActionResult> _EditCollectionFailedResponseAsync(CollectionDto collection, EditCollectionRequest request)
        {
            var response = collection.Adapt<EditCollectionRequest>();
            response.Name = request.Name;
            response.Description = request.Description;

            return View(response);
        }
    }
}
