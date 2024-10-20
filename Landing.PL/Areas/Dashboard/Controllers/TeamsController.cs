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
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public TeamsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var Teams = context.Teams.ToList();
            return View(mapper.Map<IEnumerable<TeamVM>>(Teams));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TeamFormVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TeamFormVM vm)
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

            var Team = mapper.Map<Team>(vm);
            context.Teams.Add(Team);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Team = context.Teams.Find(id);
            if (Team == null)
            {
                return NotFound();
            }

            var TeamFormVM = mapper.Map<TeamFormVM>(Team);
            return View(TeamFormVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TeamFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var Team = context.Teams.Find(vm.Id);
            if (Team == null)
            {
                return NotFound();
            }

            if (vm.Image != null)
            {
                // If a new image is uploaded, delete the old one
                FilesSettings.DeleteFile(Team.ImageName, "images");
                vm.ImageName = FilesSettings.UploadFile(vm.Image, "images");
            }

            mapper.Map(vm, Team);
            context.Teams.Update(Team);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var Team = context.Teams.Find(id);
            if (Team == null)
            {
                return NotFound();
            }

            return View(mapper.Map<TeamDetailsVM>(Team));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var Team = context.Teams.Find(id);
            if (Team == null)
            {
                return RedirectToAction(nameof(Index));
            }

            FilesSettings.DeleteFile(Team.ImageName, "images");
            context.Teams.Remove(Team);
            context.SaveChanges();

            return Ok(new { message = "Team deleted" });
        }
    }
}
