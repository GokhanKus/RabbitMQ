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

			channel.BasicQos(0, 1, false);

			var consumer = new EventingBasicConsumer(channel); //consumer = subscriber

			var queueName = channel.QueueDeclare().QueueName; //Error, Info, Warning, artik hangisini dinlemek istiyorsak onu kuyruga aktarip mesajlari gorebiliriz
			var routeKey = $"#.Error";
			// *.Error.* ortası error olan mesajları dinler, #.Error sonu Error olanları dinler Error.# basi error olanlari dinler
			// error yerine, warning, info, critical vs.' de yazilabilir.
			channel.QueueBind(queueName, "logs-topic", routeKey);

			channel.BasicConsume(queueName, false, consumer);//teslim edilen mesajlar silinmesin false olsun asagida event icerisinde biz sileriz basicAck()..
			Console.WriteLine("loglar dinleniyor");

			consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
			{
				var message = Encoding.UTF8.GetString(e.Body.ToArray());
				Thread.Sleep(1000); //cmdden 2 adet subscriber calistirirsak mesela mesajların sırasıyla subscriberlara gidecegini goruruz.
									//cd E:\Udemy_Projects\RabbitMQ\RabbitMQ.Subscriber dotnet run
				Console.WriteLine("Gelen mesaj: " + message);
				//File.AppendAllText("log-critical.txt", message + "\n"); //critical seviyesindeki logları txt dosyasina yazdiralim
				channel.BasicAck(e.DeliveryTag, false);
			};


			Console.ReadLine();
		}
	}
}
