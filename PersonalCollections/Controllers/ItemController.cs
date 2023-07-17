using System.Security.Claims;
using Application.Common.Contracts.Services;
using Application.Models.Email;
using Application.Models.Item;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalCollections.Models;

namespace PersonalCollections.Controllers
{

    public class ItemController : Controller {
        private readonly IItemService _itemService;
        private readonly ICollectionService _collectionService;

        public ItemController(IItemService itemService, ICollectionService collectionService) {
            _itemService = itemService;
            _collectionService = collectionService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddItem(string collectionId) {
            if (await _ValidateAuthorAsync(collectionId))
            {
                return Forbid();
            }

            var fields = await _itemService.GetItemFieldsAsync(collectionId);
            var request = new AddItemRequest {
                CollectionId = collectionId,
                Fields = fields
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddItem(AddItemRequest request) {
            if(!ModelState.IsValid) {
                return View(request);
            }

            if(await _ValidateAuthorAsync(request.CollectionId))
            {
                return Forbid();
            }

            var response = await _itemService.AddItemAsync(request);
            if (!response.Succeeded) {
                foreach(var error in response.Errors) {
                    ModelState.AddModelError("", error);
                }
                var fields = await _itemService.GetItemFieldsAsync(request.CollectionId);
                request.Fields = fields;
                return View(request);
            }

            return RedirectToAction(nameof(CollectionController.Collection), "Collection", new { collectionId = request.CollectionId });
        }

        [HttpGet]
        public async Task<IActionResult> TagAutocomplete([FromQuery] string term) {
            var tags = await _itemService.GetTagsByPrefixAsync(term);

            return Json(tags.Select(x => x.Name).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Items(GetItemsRequest request, CancellationToken cancellationToken = default) {
            var items = await _itemService.GetItemsAsync(request, cancellationToken);
            var collection = await _collectionService.GetByIdAsync(request.CollectionId);

            ViewData["page"] = request.Page;
            ViewData["total"] = (int)Math.Ceiling((double)(await _itemService.CountItemsAsync(request.CollectionId, request.Filter, request.DateEntries, cancellationToken)) / request.PageSize);

            return PartialView("_ItemsPartial", new GetItemsResponse
            {
                UserId = collection.UserId,
                Items = items
            });
        }

        [HttpGet]
        public async Task<IActionResult> Item(string itemId)
        {
            var item = await _itemService.GetByIdAsync(itemId);

            if(item is null)
            {
                return View("Error", new ErrorViewModel { Message = "Item not found!" });
            }
            var collection = await _collectionService.GetByIdAsync(item.CollectionId);
            var likes = await _itemService.CountLikesAsync(itemId);
            var isLiked = false;

            if (User.Identity?.IsAuthenticated ?? false)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                isLiked = await _itemService.IsLikedAsync(userId, itemId);
            }

            return View(new ItemResponse
            {
                Collection = collection,
                Item = item,
                Likes = likes,
                IsLiked = isLiked
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveItem(string itemId)
        {
            var validateResult = await _ValidateAuthorByItemIdAsync(itemId);
            if(validateResult.ActionResult is not null)
            {
                return validateResult.ActionResult;
            }

            var result = await _itemService.RemoveAsync(itemId);
            return result.Succeeded ? Ok() : BadRequest();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditItem(string itemId)
        {
            var validateResult = await _ValidateAuthorByItemIdAsync(itemId);
            if(validateResult.ActionResult is not null)
            {
                return validateResult.ActionResult;
            }

            validateResult.Item.Fields = validateResult.Item.Fields.OrderBy(x => x.CollectionField.Order).ToList();
            return View(validateResult.Item);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditItem(ItemDto request)
        {
            var validateResult = await _ValidateAuthorByItemIdAsync(request.Id);
            if (validateResult.ActionResult is not null)
            {
                return validateResult.ActionResult;
            }

            await _itemService.UpdateItemAsync(request);
            return RedirectToAction("Collection", "Collection", new { collectionId = request.CollectionId });
        }

        [HttpGet]
        public async Task<IActionResult> Comments(string itemId, int skip, int pageSize)
        {
            var comments = await _itemService.GetCommentsAsync(itemId, skip, pageSize);
            return PartialView("_CommentsPartial", comments);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(term))
            {
                return View(new SearchItemsResponse());
            }
            var items = await _itemService.GetItemsAsync(term, page: 1, pageSize, cancellationToken);

            return View(new SearchItemsResponse
            {
                Term = term,
                Items = items
            });
        }

        private async Task<bool> _ValidateAuthorAsync(string collectionId)
        {
            var collection = await _collectionService.GetByIdAsync(collectionId);
            if (collection is null)
            {
                return false;
            }

            return !User.IsInRole("Admin") && User.FindFirstValue(ClaimTypes.NameIdentifier) != collection.UserId;
        }

        private async Task<ValidateAuthorResult> _ValidateAuthorByItemIdAsync(string itemId)
        {
            var item = await _itemService.GetByIdAsync(itemId);

            if(item is null)
            {
                return new ValidateAuthorResult { 
                    ActionResult = View("Error", new ErrorViewModel { Message = "Item not found!" }) 
                };
            }

            if(await _ValidateAuthorAsync(item.CollectionId))
            {
                return new ValidateAuthorResult
                {
                    ActionResult = Forbid()
                };
            }

            return new ValidateAuthorResult
            {
                Item = item
            };
        }

        private class ValidateAuthorResult
        {
            public IActionResult? ActionResult { get; set; }
            public ItemDto? Item { get; set; }
        }
    }
}
