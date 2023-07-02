using System.Security.Claims;
using Application.Common.Contracts.Services;
using Application.Models.Collection;
using Microsoft.AspNetCore.Mvc;

namespace PersonalCollections.Controllers
{

    public class CollectionController : Controller
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public IActionResult AddCollection()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCollection(AddCollectionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var userId = TempData.Peek("UserId")?.ToString();
            //request.UserId = userId;
            request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _collectionService.AddAsync(request);

            return Ok();
        }
    }
}
