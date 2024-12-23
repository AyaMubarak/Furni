﻿using Landing.DAL.Data;
using Landing.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Landing.PL.Controllers
{
	public class ServiceController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ServiceController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var products = await _context.Services.ToListAsync();
			var model = products.Select(p => new ServiceFormVM
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
