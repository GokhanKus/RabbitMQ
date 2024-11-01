﻿using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQWeb.Watermark.Services
{
	public class RabbitMQPublisher
	{
		private readonly RabbitMQClientService _rabbitMQClientService;
		public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
		{
			_rabbitMQClientService = rabbitMQClientService;
		}
		public void Publish(ProductImageCreatedEvent productImageCreatedEvent)
		{
			var channel = _rabbitMQClientService.Connect();

			var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);
			var bodyByte = Encoding.UTF8.GetBytes(bodyString);

			var properties = channel.CreateBasicProperties();
			properties.Persistent = true; //mesajlar kalici olsun

			channel.BasicPublish(exchange: RabbitMQClientService.exchangeName,
								 routingKey: RabbitMQClientService.routingWaterMark,
								 basicProperties: properties,
								 body: bodyByte);
		}
	}
}
