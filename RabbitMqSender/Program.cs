using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMqCommon;

namespace RabbitMqSender
{
	class Program
	{
		static void Main()
		{
			bool usingRabbitMqCommonHelper = false;
			if (!usingRabbitMqCommonHelper)
			{
				var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
				using (var connection = factory.CreateConnection())
				using (var channel = connection.CreateModel())
				{
					channel.QueueDeclare(queue: "hello",
										 durable: false,
										 exclusive: false,
										 autoDelete: false,
										 arguments: null);

					string message = "Hello World!";
					var body = Encoding.UTF8.GetBytes(message);

					channel.BasicPublish(exchange: "",
										 routingKey: "hello",
										 basicProperties: null,
										 body: body);
					Console.WriteLine(" [x] Sent {0}", message);
				}

				Console.WriteLine(" Press [enter] to exit.");
				Console.ReadLine();
			}
			else
			{
				RabbitMQClientMgr rabbitMQClientMgr = RabbitMqCommonHelper.Initialize();

				string message = "Hello World!!";
				var body = Encoding.UTF8.GetBytes(message);
				rabbitMQClientMgr.BasicPublish(body);

				Console.WriteLine(" [x] Sent {0}", message);

				Console.WriteLine(" Press [enter] to exit.");
				Console.ReadLine();

				rabbitMQClientMgr.Uninitialize();
			}
		}
	}
}
