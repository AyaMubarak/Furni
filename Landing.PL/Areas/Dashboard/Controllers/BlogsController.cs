using AutoMapper;
using Landing.DAL.Data;
using Landing.DAL.Models;
using Landing.PL.Areas.Dashboard.ViewModels;
using Landing.PL.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Landing.PL.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BlogsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var Blogs = context.Blogs.ToList();
            return View(mapper.Map<IEnumerable<BlogVM>>(Blogs));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new BlogFormVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Check if the image file is valid
            if (vm.Image != null)
            {
                vm.ImageName = FilesSettings.UploadFile(vm.Image, "images");
            }
            else
            {
                ModelState.AddModelError("Image", "Image is required.");
                return View(vm);
            }

            var Blog = mapper.Map<Blog>(vm);
            context.Blogs.Add(Blog);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Blog = context.Blogs.Find(id);
            if (Blog == null)
            {
                return NotFound();
            }

            var BlogFormVM = mapper.Map<BlogFormVM>(Blog);
            return View(BlogFormVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BlogFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var Blog = context.Blogs.Find(vm.Id);
            if (Blog == null)
            {
                return NotFound();
            }

            if (vm.Image != null)
            {
                // If a new image is uploaded, delete the old one
                FilesSettings.DeleteFile(Blog.ImageName, "images");
                vm.ImageName = FilesSettings.UploadFile(vm.Image, "images");
            }

            mapper.Map(vm, Blog);
            context.Blogs.Update(Blog);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var Blog = context.Blogs.Find(id);
            if (Blog == null)
            {
                return NotFound();
            }

            return View(mapper.Map<BlogDetailsVM>(Blog));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var Blog = context.Blogs.Find(id);
            if (Blog == null)
            {
                return RedirectToAction(nameof(Index));
            }

            FilesSettings.DeleteFile(Blog.ImageName, "images");
            context.Blogs.Remove(Blog);
            context.SaveChanges();

            return Ok(new { message = "Blog deleted" });
        }
    }
}
