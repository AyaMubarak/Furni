using Landing.DAL.Data;
using Landing.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Landing.PL.Controllers
{
	public class ShopController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ShopController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var products = await _context.Shops.ToListAsync();
			var model = products.Select(p => new ShopFormVM
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
				ImageName= p.ImageName,
				Description = p.Description,

			});

			return View(model);
		}
		public IActionResult Details(int id)
		{
			var Post = _context.Shops.Find(id); 
			if (Post == null)
			{
				return NotFound();
			}

			var viewModel = new ShopFormVM
			{
				Id = Post.Id,
				Name = Post.Name,
				ImageName = Post.ImageName,
				Description = Post.Description, 
				Price= Post.Price,
			};

			return View(viewModel);
		}
        public async Task<ShopFormVM> GetProductById(int id)
        {
            var shop = await _context.Shops
                .Where(p => p.Id == id)
                .Select(p => new ShopFormVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageName = p.ImageName,
                    Description = p.Description
                })
                .FirstOrDefaultAsync();

            return shop;
        }

    }
}
