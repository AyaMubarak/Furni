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
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ServicesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var services = context.Services.ToList();
            return View(mapper.Map<IEnumerable<ServiceVM>>(services));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ServiceFormVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceFormVM vm)
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

            var service = mapper.Map<Service>(vm);
            context.Services.Add(service);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var service = context.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            var serviceFormVM = mapper.Map<ServiceFormVM>(service);
            return View(serviceFormVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ServiceFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var service = context.Services.Find(vm.Id);
            if (service == null)
            {
                return NotFound();
            }

            if (vm.Image != null)
            {
                // If a new image is uploaded, delete the old one
                FilesSettings.DeleteFile(service.ImageName, "images");
                vm.ImageName = FilesSettings.UploadFile(vm.Image, "images");
            }

            mapper.Map(vm, service);
            context.Services.Update(service);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var service = context.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            return View(mapper.Map<ServiceDetailsVM>(service));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var service = context.Services.Find(id);
            if (service == null)
            {
                return RedirectToAction(nameof(Index));
            }

            FilesSettings.DeleteFile(service.ImageName, "images");
            context.Services.Remove(service);
            context.SaveChanges();

            return Ok(new { message = "Service deleted" });
        }
    }
}
