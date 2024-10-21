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

			//0: her turlu boyutta mesaj gonderebilirsin demek,
			//1 ve false diyerekte kaç tane subscriber varsa yani o mesajı alacak kaç tane tuketici varsa, sırasıyla 1'er 1'er gonderir

			channel.BasicQos(0, 1, false);

			//consumer = subscriber 
			var consumer = new EventingBasicConsumer(channel);

			channel.BasicConsume(queueName, false, consumer);//teslim edilen mesajlar silinmesin false olsun asagida event icerisinde biz sileriz basicAck()..

			consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
			{
				var message = Encoding.UTF8.GetString(e.Body.ToArray());
				Thread.Sleep(1500); //cmdden 2 adet subscriber calistirirsak mesela mesajların sırasıyla subscriberlara gidecegini goruruz.
				//cd E:\Udemy_Projects\RabbitMQ\RabbitMQ.Subscriber dotnet run
				Console.WriteLine("Gelen mesaj: " + message);
				channel.BasicAck(e.DeliveryTag, false);
			};
			Console.ReadLine();
		}
	}
}
