using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

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

			#region Header Exchange
			// header exchange'de key value seklinde olur header'da mesela format=pdf, shape=a4 olsun
			// mesajı talep ederken consumer tarafında header=> format=pdf ve shape=a4 diyerek ilgili mesajları alırız
			// ek olarak x-match = any derken formatı pdf olan veya shape'i a4 olanlar gelir
			// ama x-match = all dersek hem pdf hem de a4 olacak anlamina gelir

			#endregion

			channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

			Dictionary<string, object> headers = new Dictionary<string, object>();
			headers.Add("format", "pdf");
			headers.Add("shape", "a4");

			var properties = channel.CreateBasicProperties();
			properties.Headers = headers;
			properties.Persistent = true; //queueleri durable true yaparak kalıcı hale getirebildigimiz gibi mesajları da bu sekilde kalıcı hale getirebiliriyoruz

			//bu zamana kadar mesajlari string tipinde gonderdik ama pdf, img, ya da bir class instance'ı da gonderebiliriz(complex type'lari mesaj olarak gondermek)
			//var product2 = new Product { Id = 1, Stock = 50 , Name = "Silgi", Price = 25 };
			var product = new Product(1, 150, "Kalem", 10);
			var objectToSerialized = JsonSerializer.Serialize(product);
			channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes(objectToSerialized));

			Console.WriteLine("mesaj gonderilmistir");
			Console.ReadLine();
		}
	}
}
