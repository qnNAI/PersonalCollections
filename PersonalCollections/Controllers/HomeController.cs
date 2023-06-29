using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalCollections.Models;
using System.Diagnostics;

namespace PersonalCollections.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		//[Authorize]
		public IActionResult Index()
		{
			return View();
		}

		[Authorize(Roles = "User")]
		public IActionResult Test() {
			return View("Error");
		}
	}
}