using RabbitMQ.Client;

namespace RabbitMQWeb.Excel.Services
{
	public class RabbitMQClientService : IDisposable
	{
		private readonly ConnectionFactory _connectionFactory;
		private IConnection _connection;
		private IModel _channel;

		//direct exchange kullanacagiz
		public static string ExchangeName = "ExcelDirectExchange";
		public static string RoutingExcel = "excel-route-file";
		public static string QueueName = "queue-excel-file";

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
			_channel.ExchangeDeclare(ExchangeName, type: "direct", true, false);
			_channel.QueueDeclare(QueueName, true, false, false);
			_channel.QueueBind(QueueName, ExchangeName, RoutingExcel);
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
