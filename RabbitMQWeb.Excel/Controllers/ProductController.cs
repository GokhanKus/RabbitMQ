﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using RabbitMQWeb.Excel.Models;
using RabbitMQWeb.Excel.Services;
using Shared;

namespace RabbitMQWeb.Excel.Controllers
{
	[Authorize]
	public class ProductController : Controller
	{
		private readonly AppDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RabbitMQPublisher _rabbitMQPublisher;

		public ProductController(AppDbContext context, UserManager<IdentityUser> userManager, RabbitMQPublisher rabbitMQPublisher)
		{
			_context = context;
			_userManager = userManager;
			_rabbitMQPublisher = rabbitMQPublisher;
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

			_rabbitMQPublisher.Publish(new CreateExcelMessage { FileId = userFile.Id});

			TempData["StartCreatingExcel"] = true;

			return RedirectToAction(nameof(Files));
		}
		public async Task<IActionResult> Files()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			var userFiles = await _context.UserFiles.Where(x => x.UserId == user.Id).OrderByDescending(uf=>uf.Id).ToListAsync();
			return View(userFiles);
		}
	}
}
