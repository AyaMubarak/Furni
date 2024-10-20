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
    public class TestimonialsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public TestimonialsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var Testimonials = context.Testimonials.ToList();
            return View(mapper.Map<IEnumerable<TestimonialVM>>(Testimonials));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TestimonialFormVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TestimonialFormVM vm)
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

            var Testimonial = mapper.Map<Testimonial>(vm);
            context.Testimonials.Add(Testimonial);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Testimonial = context.Testimonials.Find(id);
            if (Testimonial == null)
            {
                return NotFound();
            }

            var TestimonialFormVM = mapper.Map<TestimonialFormVM>(Testimonial);
            return View(TestimonialFormVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TestimonialFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var Testimonial = context.Testimonials.Find(vm.Id);
            if (Testimonial == null)
            {
                return NotFound();
            }

            if (vm.Image != null)
            {
                // If a new image is uploaded, delete the old one
                FilesSettings.DeleteFile(Testimonial.ImageName, "images");
                vm.ImageName = FilesSettings.UploadFile(vm.Image, "images");
            }

            mapper.Map(vm, Testimonial);
            context.Testimonials.Update(Testimonial);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var Testimonial = context.Testimonials.Find(id);
            if (Testimonial == null)
            {
                return NotFound();
            }

            return View(mapper.Map<TestimonialDetailsVM>(Testimonial));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var Testimonial = context.Testimonials.Find(id);
            if (Testimonial == null)
            {
                return RedirectToAction(nameof(Index));
            }

            FilesSettings.DeleteFile(Testimonial.ImageName, "images");
            context.Testimonials.Remove(Testimonial);
            context.SaveChanges();

            return Ok(new { message = "Testimonial deleted" });
        }
    }
}
