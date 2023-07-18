using Application.Common.Contracts.Services;
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
			var items = await _itemService.GetLatestItemsAsync(pageSize: 10, cancellationToken);

			return View();
		}
	}
}