using FileCreateWorkerService.Services;
using RabbitMQ.Client;

namespace FileCreateWorkerService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = Host.CreateApplicationBuilder(args);
			builder.Services.AddHostedService<Worker>();

			var amqpUrl = builder.Configuration.GetConnectionString("RabbitMQ");
			builder.Services.AddSingleton(sp => new ConnectionFactory { Uri = new Uri(amqpUrl), DispatchConsumersAsync = true });
			builder.Services.AddSingleton<RabbitMQClientService>();

			var host = builder.Build();
			host.Run();
		}
	}
}
// excel dosyasini olusturmak icin workerservice projesi olusturuldu