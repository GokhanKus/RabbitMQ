﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Text;
using System.Text.Json;

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

			channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);
			//queue ve exchangeleri subscriber(consumer) ya da publisher'da yani tek birinde declare etmemiz yeterli
			channel.BasicQos(0, 1, false);

			var consumer = new EventingBasicConsumer(channel); //consumer = subscriber

			var queueName = channel.QueueDeclare().QueueName; //Error, Info, Warning, artik hangisini dinlemek istiyorsak onu kuyruga aktarip mesajlari gorebiliriz

			Dictionary<string, object> headers = new Dictionary<string, object>();

			headers.Add("format", "pdf");
			headers.Add("shape", "a4");
			headers.Add("x-match", "all");

			channel.QueueBind(queueName, "header-exchange", string.Empty, headers);

			channel.BasicConsume(queueName, false, consumer);//teslim edilen mesajlar silinmesin false olsun asagida event icerisinde biz sileriz basicAck()..
			Console.WriteLine("loglar dinleniyor");

			consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
			{
				var message = Encoding.UTF8.GetString(e.Body.ToArray());
				//producer tarafinda bir class instance'ını serilize edip byte dizisine cevirdik ve yayinladik. Burada da deserilize ederek mesaji okuyoruz
				var p = JsonSerializer.Deserialize<Product>(message);
				Thread.Sleep(1000);
				Console.WriteLine($"class instance'ı gelen mesaj: \nId:{p.Id} \nStock:{p.Stock} \nName:{p.Name} \nPrice:{p.Price}");
				channel.BasicAck(e.DeliveryTag, false);
			};


			Console.ReadLine();
		}
	}
}
