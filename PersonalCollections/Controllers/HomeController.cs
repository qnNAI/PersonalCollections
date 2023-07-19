using Application.Common.Contracts.Services;
using Application.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalCollections.Models;
using System.Diagnostics;

namespace PersonalCollections.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IItemService _itemService;
        private readonly ICollectionService _collectionService;

        public HomeController(ILogger<HomeController> logger, IItemService itemService, ICollectionService collectionService)
		{
			_logger = logger;
            _itemService = itemService;
            _collectionService = collectionService;
        }

		public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
		{
			var itemsTask = _itemService.GetLatestItemsAsync(pageSize: 10, cancellationToken);
			var collectionsTask = _collectionService.GetLargestCollectionsAsync(pageSize: 5, cancellationToken);
			var tagsTask = _itemService.GetLargestTags(pageSize: 20, cancellationToken);

			return View(new MainPageResponse
			{
				Items = await itemsTask,
				Collections = await collectionsTask,
				Tags = await tagsTask
            });
		}
	}
}