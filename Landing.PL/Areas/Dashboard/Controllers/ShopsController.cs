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
    public class ShopsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ShopsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var Shops = context.Shops.ToList();
            return View(mapper.Map<IEnumerable<ShopVM>>(Shops));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ShopFormVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ShopFormVM vm)
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

            var Shop = mapper.Map<Shop>(vm);
            context.Shops.Add(Shop);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Shop = context.Shops.Find(id);
            if (Shop == null)
            {
                return NotFound();
            }

            var ShopFormVM = mapper.Map<ShopFormVM>(Shop);
            return View(ShopFormVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ShopFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var Shop = context.Shops.Find(vm.Id);
            if (Shop == null)
            {
                return NotFound();
            }

            if (vm.Image != null)
            {
                // If a new image is uploaded, delete the old one
                FilesSettings.DeleteFile(Shop.ImageName, "images");
                vm.ImageName = FilesSettings.UploadFile(vm.Image, "images");
            }

            mapper.Map(vm, Shop);
            context.Shops.Update(Shop);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var Shop = context.Shops.Find(id);
            if (Shop == null)
            {
                return NotFound();
            }

            return View(mapper.Map<ShopDetailsVM>(Shop));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var Shop = context.Shops.Find(id);
            if (Shop == null)
            {
                return RedirectToAction(nameof(Index));
            }

            FilesSettings.DeleteFile(Shop.ImageName, "images");
            context.Shops.Remove(Shop);
            context.SaveChanges();

            return Ok(new { message = "Shop deleted" });
        }
    }
}
