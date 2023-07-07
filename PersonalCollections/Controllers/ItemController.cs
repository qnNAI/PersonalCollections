using Application.Common.Contracts.Services;
using Application.Models.Item;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddItem(string collectionId) {
            var fields = await _itemService.GetItemFieldsAsync(collectionId);
            var request = new AddItemRequest {
                CollectionId = collectionId,
                Fields = fields
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(AddItemRequest request) {
            if(!ModelState.IsValid) {
                return View(request);
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
            ViewData["total"] = (int)Math.Ceiling((double)(await _itemService.CountItemsAsync(request.CollectionId, request.Filter, cancellationToken)) / request.PageSize);

            return PartialView("_ItemsPartial", new GetItemsResponse
            {
                UserId = collection.UserId,
                Items = items
            });
        }
    }
}
