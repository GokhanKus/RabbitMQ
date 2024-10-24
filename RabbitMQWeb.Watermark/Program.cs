using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQWeb.Watermark.Models;
using RabbitMQWeb.Watermark.Services;

namespace RabbitMQWeb.Watermark
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddDbContext<AppDbContext>(options =>
			{
				options.UseInMemoryDatabase(databaseName: "productDb");
			});
			builder.Services.AddControllersWithViews();

			var amqpUrl = builder.Configuration.GetConnectionString("RabbitMQ");

			builder.Services.AddSingleton(sp => new ConnectionFactory { Uri = new Uri(amqpUrl) });
			builder.Services.AddSingleton<RabbitMQClientService>();
			builder.Services.AddSingleton<RabbitMQPublisher>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
