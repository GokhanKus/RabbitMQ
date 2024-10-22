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

			#region Direct Exchange
			//Direct exchange'deki senaryo için örnegin critical, error, warning, info seklinde routelerimiz olabilir bunlar queue'den consumerlere aktarilir
			//ilgili seviyelerdeki mesajlar kuyrukta bekler ve mesela critical route db'ye error route dosya olarak kaydedilebilir
			//fanout exchange'den farklı olarak bunda route key vardır. ve queueler publisher tarafından olusturulur?
			#endregion

			channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

			Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
			{
				var routeKey = $"route - {x}";
				var queueName = $"direct queue - {x}";

				channel.QueueDeclare(queueName, true, false, false);
				channel.QueueBind(queueName, "logs-direct", routeKey, null);
			});

			Enumerable.Range(1, 50).ToList().ForEach(x =>
			{
				var log = (LogNames)new Random().Next(1, 5);
				string message = $"log-type: {log}";
				var messageBody = Encoding.UTF8.GetBytes(message);
				var routeKey = $"route - {log}";

				channel.BasicPublish("logs-direct", routeKey, null, messageBody);
				Console.WriteLine($"log gonderilmistir: {message}");
			});

			Console.ReadLine();
		}
	}
}
