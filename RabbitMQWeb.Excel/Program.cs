using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQWeb.Excel.Extensions;
using RabbitMQWeb.Excel.Models;
using RabbitMQWeb.Excel.Services;

namespace RabbitMQWeb.Excel
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			
			builder.Services.AddDbContext<AppDbContext>(options =>
			{
				var connectionString = builder.Configuration.GetConnectionString("SqlServer");
				options.UseSqlServer(connectionString);
			});

			var amqpUrl = builder.Configuration.GetConnectionString("RabbitMQ");

			builder.Services.AddSingleton(sp => new ConnectionFactory { Uri = new Uri(amqpUrl), DispatchConsumersAsync = true });
			builder.Services.AddSingleton<RabbitMQClientService>();

			builder.Services.AddIdentity<IdentityUser, IdentityRole>(opt =>
			{
				opt.User.RequireUniqueEmail = true;
			}).AddEntityFrameworkStores<AppDbContext>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.ConfigureDefaultAdminUser();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
