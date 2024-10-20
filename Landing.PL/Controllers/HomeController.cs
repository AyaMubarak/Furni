using Landing.DAL.Data;
using Landing.PL.Areas.Dashboard.ViewModels;
using Landing.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Landing.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch all entities asynchronously
            var blogs = await _context.Blogs.ToListAsync();
            var services = await _context.Services.ToListAsync();
            var shops = await _context.Shops.ToListAsync();
            var teams = await _context.Teams.ToListAsync();
            var testimonials = await _context.Testimonials.ToListAsync();

            // Create a combined model
            var viewModel = new CombinedViewModel
            {
                Blogs = blogs.Select(b => new BlogFormVM
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    ImageName = b.ImageName,
                    CreatedAt = b.CreatedAt,
                    CreatedBy = b.CreatedBy,
                }).ToList(),
                Services = services.Select(s => new ServiceFormVM
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ImageName = s.ImageName,
                }).ToList(),
                Shops = shops.Select(s => new ShopFormVM
                {
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    ImageName = s.ImageName,
                    Description = s.Description,
                }).ToList(),
                Teams = teams.Select(t => new TeamFormVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    ImageName = t.ImageName,
                }).ToList(),
                Testimonials = testimonials.Select(t => new TestimonialFormVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    ImageName = t.ImageName,
                }).ToList(),
            };

            return View(viewModel);
        }
    }
}
