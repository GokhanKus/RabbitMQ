using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.Publisher
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var factory = new ConnectionFactory();
			string amqpUrl = "amqps://opmzghqv:Gcm-XamhxS2oH2oNCdgYhmApsU1oUeol@rat.rmq2.cloudamqp.com/opmzghqv";
			factory.Uri = new Uri(amqpUrl);

			using var connection = factory.CreateConnection();

			var channel = connection.CreateModel();

			channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);
			#region Fanout Exchange
			//fanout exchange'deki senaryo için her consumer kendi queue'lerini olusturur, consumerlar down oldugu zaman queueleri de silinir.
			//eger hic consumer yoksa queue'de olmaz ve yayinlanan mesajlar bosa gider; ki zaten talep eden olmadigi icin sorun yok
			#endregion

			Enumerable.Range(1, 50).ToList().ForEach(x =>
			{
				string message = $"log {x}";
				var messageBody = Encoding.UTF8.GetBytes(message);
				channel.BasicPublish("logs-fanout", string.Empty, null, messageBody);
				Console.WriteLine($"message has been sent: {message}");
			});

			Console.ReadLine();
		}
	}
}
