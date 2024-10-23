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

			#region Topic Exchange
			// detaylı routelama yapilacagi zaman kullanilir, routing key "." larla belirtilir; örneğin routing key Critical.Error.Warning olsun 
			//burada queueleri consumer olusturur, cunku varyasyon cok fazla olabilir yani routing key'de 3 oge yerine daha fazla, daha az veya sırasi farklı olabilir
			//consumerlar(subscriber) istedigi routekey'i belirterek ilgili mesaji alabilir mesela
			// routing keyi = *.Error* olarak belirtirse ortası Error olsun digerleri ne olursa olsun
			// routing keyi = #.Error olursa da sonu Errorla bitenler gelsin demek..
			#endregion

			channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);

			Random rnd = new Random();

			Enumerable.Range(1, 50).ToList().ForEach(x =>
			{
				var log1 = (LogNames)rnd.Next(1, 5);
				var log2 = (LogNames)rnd.Next(1, 5);
				var log3 = (LogNames)rnd.Next(1, 5);

				var routeKey = $"{log1}.{log2}.{log3}";
				string message = $"log-type: {log1}-{log2}-{log3}";
				var messageBody = Encoding.UTF8.GetBytes(message);

				channel.BasicPublish("logs-topic", routeKey, null, messageBody);
				Console.WriteLine($"log gonderilmistir: {message}");
			});

			Console.ReadLine();
		}
	}
}
