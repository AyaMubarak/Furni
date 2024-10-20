using Landing.DAL.Data;
using Landing.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Landing.PL.Controllers
{
	public class TeamController : Controller
	{
		private readonly ApplicationDbContext _context;

		public TeamController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var products = await _context.Teams.ToListAsync();
			var model = products.Select(p => new TeamFormVM
			{
				Id = p.Id,
				Name = p.Name,
				Description = p.Description,
				ImageName = p.ImageName,

			});

			return View(model);
		}
	}
}
