﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using RabbitMQWeb.Excel.Models;

namespace RabbitMQWeb.Excel.Controllers
{
	[Authorize]
	public class ProductController : Controller
	{
		private readonly AppDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public ProductController(AppDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> CreateProductExcel()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			var fileName = $"product-excel-{Guid.NewGuid().ToString().Substring(1, 10)}";
			UserFile userFile = new()
			{
				UserId = user.Id,
				FileName = fileName,
				FileStatus = FileStatus.Creating
			};
			await _context.UserFiles.AddAsync(userFile);
			await _context.SaveChangesAsync();
			//rabbitMQ'ya mesaj gonderilecek
			TempData["StartCreatingExcel"] = true;

			return RedirectToAction(nameof(Files));
		}
		public async Task<IActionResult> Files()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			var userFiles = await _context.UserFiles.Where(x => x.UserId == user.Id).ToListAsync();
			return View(userFiles);
		}
	}
}