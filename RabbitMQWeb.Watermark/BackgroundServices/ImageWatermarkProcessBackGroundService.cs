using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQWeb.Watermark.Services;
using System.Drawing;
using System.Text;
using System.Text.Json;

namespace RabbitMQWeb.Watermark.BackgroundServices
{
	public class ImageWatermarkProcessBackGroundService : BackgroundService
	{
		private readonly RabbitMQClientService _rabbitmqClientService;
		private readonly ILogger<ImageWatermarkProcessBackGroundService> _logger;
		private IModel _channel;
		public ImageWatermarkProcessBackGroundService(RabbitMQClientService rabbitmqClientService, ILogger<ImageWatermarkProcessBackGroundService> logger)
		{
			_rabbitmqClientService = rabbitmqClientService;
			_logger = logger;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			_channel = _rabbitmqClientService.Connect();
			_channel.BasicQos(0, 1, false);

			return base.StartAsync(cancellationToken);
		}
		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var consumer = new AsyncEventingBasicConsumer(_channel);
			_channel.BasicConsume(RabbitMQClientService.queueName, false, consumer);

			consumer.Received += Consumer_Received;

			return Task.CompletedTask;
		}

		private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
		{
			//burada 10 saniye bekletiyoruz sonra create asamasinda eklenen resim arkaplanda resme yazı ekleme islemi asenkron bir sekilde gerçeklesiyor
			Task.Delay(10000).Wait();
			try
			{
				//resme yazı ekleme islemi burada gerceklesecek
				var productImageCreatedEvent = JsonSerializer.Deserialize<ProductImageCreatedEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", productImageCreatedEvent.ImageName);

				string websiteName = "www.mywebsite.com";

				using var img = Image.FromFile(path);
				using var graphic = Graphics.FromImage(img);

				var font = new Font(FontFamily.GenericMonospace, 32, FontStyle.Bold, GraphicsUnit.Pixel);
				var textSize = graphic.MeasureString(websiteName, font);

				var color = Color.FromArgb(128, 255, 255, 255);
				var brush = new SolidBrush(color);

				var position = new Point(img.Width - ((int)textSize.Width + 30), img.Height - ((int)textSize.Height + 30));

				graphic.DrawString(websiteName, font, brush, position);
				img.Save("wwwroot/images/watermarks/" + productImageCreatedEvent.ImageName);

				img.Dispose();
				graphic.Dispose();

				_channel.BasicAck(@event.DeliveryTag, false);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			return Task.CompletedTask;
		}

		public override Task StopAsync(CancellationToken cancellationToken)
		{
			//RabbitMQClientService.cs tarafında zaten dispose olacagi icin burada bir sey yapmamiza gerek yok
			return base.StopAsync(cancellationToken);
		}
	}
}
