using Landing.DAL.Data;
using Landing.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Landing.PL.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Blogs.ToListAsync();
            var model = products.Select(p => new BlogFormVM
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImageName = p.ImageName,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
            });

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var blogPost = _context.Blogs.Find(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            var viewModel = new BlogFormVM
            {
                Id = blogPost.Id,
                Name = blogPost.Name,
                ImageName = blogPost.ImageName,
                CreatedBy = blogPost.CreatedBy,
                CreatedAt = blogPost.CreatedAt,
                Description = blogPost.Description, 
            };

            return View(viewModel);
        }
    }
}
