using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQWeb.Excel.Models;

namespace RabbitMQWeb.Excel.Extensions
{
	public static class ApplicationExtension
	{
		public static void ConfigureDefaultAdminUser(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

				//appDbContext.Database.Migrate();

				if (!appDbContext.Users.Any())
				{
					userManager.CreateAsync(new IdentityUser() { UserName = "user1", Email = "user1@hotmail.com" }, "User.123456").Wait();
					userManager.CreateAsync(new IdentityUser() { UserName = "user2", Email = "user2@hotmail.com" }, "User.123456").Wait();
				}
			}
		}
	}
}
