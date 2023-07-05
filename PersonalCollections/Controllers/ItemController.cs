using Application.Common.Contracts.Services;
using Application.Models.Item;
using Microsoft.AspNetCore.Mvc;

namespace PersonalCollections.Controllers
{

    public class ItemController : Controller
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> AddItem(string collectionId)
        {
            var fields = await _itemService.GetItemFieldsAsync(collectionId);
            var request = new AddItemRequest
            {
                CollectionId = collectionId,
                Fields = fields
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(AddItemRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            
            await _itemService.AddItemAsync(request);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> TagAutocomplete([FromQuery] string term)
        {
            var tags = await _itemService.GetTagsByPrefixAsync(term);

            return Json(tags.Select(x => x.Name).ToList());
        }
    }
}
