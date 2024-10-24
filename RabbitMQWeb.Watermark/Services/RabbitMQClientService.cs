using RabbitMQ.Client;

namespace RabbitMQWeb.Watermark.Services
{
	public class RabbitMQClientService : IDisposable
	{
		private readonly ConnectionFactory _connectionFactory;
		private IConnection _connection;
		private IModel _channel;

		public static string exchangeName = "ImageDirectExchange";
		public static string routingWaterMark = "watermark-route-image";
		public static string queueName = "queue-watermark-image";

		private readonly ILogger<RabbitMQClientService> _logger;
		public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
		{
			_connectionFactory = connectionFactory;
			_logger = logger;
			Connect();
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
	}
}
