using Microsoft.AspNetCore.Mvc;

namespace Landing.PL.Areas.Dashboard.Controllers
{
	[Area("Dashboard")]
	public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View(); // This should render Views/Dashboard/Index.cshtml
		}
	}
}
