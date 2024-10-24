using RabbitMQ.Client;

namespace RabbitMQWeb.Watermark.Services
{
	public class RabbitMQClientService : IDisposable
	{
		private readonly ConnectionFactory _connectionFactory;
		private IConnection _connection;
		private IModel _channel;

		//direct exchange kullanacagiz
		public static string exchangeName = "ImageDirectExchange";
		public static string routingWaterMark = "watermark-route-image";
		public static string queueName = "queue-watermark-image";

		private readonly ILogger<RabbitMQClientService> _logger;
		public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
		{
			_connectionFactory = connectionFactory;
			_logger = logger;
		}
		public IModel Connect()
		{
			_connection = _connectionFactory.CreateConnection();
			if (_channel is { IsOpen: true }) //if(_channel.IsOpen == true)
				return _channel;

			_channel = _connection.CreateModel();
			_channel.ExchangeDeclare(exchangeName, type: "direct", true, false);
			_channel.QueueDeclare(queueName, true, false, false);
			_channel.QueueBind(queueName, exchangeName, routingWaterMark);
			_logger.LogInformation("RabbitMQ ile baglanti kuruldu");

			return _channel;
		}

		public void Dispose()
		{
			_channel?.Close();
			_channel?.Dispose();

			_connection?.Close();
			_connection?.Dispose();

			_logger.LogInformation("RabbitMQ ile baglanti koptu");
		}
		#region Message&Event
		//rabbit mq'ya mesaj gonderilirken ya message ya da event tipinde gonderilir
		//Message: islenmesi icin gerekli datayı tasir ve mesaji gonderen, mesajin nasil islenecegini bilir
		//ornegin WordToPDF publisher bir word dosyasini byte dizisine cevirip message broker ile pdf'e donusturulecegini bilir sonra subscriber ile infoyu publishera doner
		//Event: eventler bir notification sistemi saglar ve eventi firlatan, onun nasıl ele alinacagini, islenecegini bilmez
		//ornegin UserCreatedEvent, OrderCreatedEvent
		//messagelar genelde islenecek olan datayı barindirirken, eventler genelde user'in id'si olabilir messagelere gore daha az maliyetlidir
		//bu ornekte event olacak. event firlatacagiz
		#endregion
	}
}
