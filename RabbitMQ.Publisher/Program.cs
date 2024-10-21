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
			string queueName = "hello-queue";

			//durable kuyrugun kalıcı olup olmadıgını belirler true olursa kuyruk kalıcı,
			//exclusive kuyrugun sadece o connection tarafından kullanılabilir oldugunu belirtir false yaparak RabbitMQ.Subscriber tarafında kullanacagiz
			//autoDelete kuyrugun otomatik olarak silinip silinmeyecegini belirtir false yaparak kuyruklar consumerlara (subscriber) ulassa bile silinmez 
			//cunku bazen mesaj dogru islenmeeyebilir o yuzden mesaj hemen silinmesin, ozetle daha dayanıklı bir yapı icin true, false, false seklinde yazılır
			channel.QueueDeclare(queue:queueName, durable: true, exclusive: false, autoDelete: false);

			string message = "first rabbitmq message";
			var messageBody = Encoding.UTF8.GetBytes(message);
			channel.BasicPublish(string.Empty, queueName, null, messageBody);

			Console.WriteLine("message has been sent");
			Console.ReadLine();
		}
	}
}
