using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.Subscriber
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

			//bu satiri publisher'da yazdigimiz icin burada tekrar tanimlamamiza gerek yok, eger orada tanimlamasaydik queue yoksa burada da olusturabilirdi.
			//channel.QueueDeclare(queueName, true, false, false);

			//consumer = subscriber 
			var consumer = new EventingBasicConsumer(channel);
			channel.BasicConsume(queueName, true, consumer);

			consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
			{
				var message = Encoding.UTF8.GetString(e.Body.ToArray());
				Console.WriteLine("Gelen mesaj: " + message);
			};
			Console.ReadLine();
		}
	}
}
